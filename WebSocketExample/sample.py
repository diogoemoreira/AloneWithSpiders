from os import device_encoding
import platform
from statistics import mean
import sys
from typing import final
import settings
import neurokit2 as nk
import numpy as np

osDic = {
    "Darwin": f"MacOS/Intel{''.join(platform.python_version().split('.')[:2])}",
    "Linux": "Linux64",
    "Windows": f"Win{platform.architecture()[0][:2]}_{''.join(platform.python_version().split('.')[:2])}",
}
if platform.mac_ver()[0] != "":
    import subprocess
    from os import linesep

    p = subprocess.Popen("sw_vers", stdout=subprocess.PIPE)
    result = p.communicate()[0].decode("utf-8").split(str("\t"))[2].split(linesep)[0]
    if result.startswith("12."):
        print("macOS version is Monterrey!")
        osDic["Darwin"] = "MacOS/Intel310"
        if (
            int(platform.python_version().split(".")[0]) <= 3
            and int(platform.python_version().split(".")[1]) < 10
        ):
            print(f"Python version required is ≥ 3.10. Installed is {platform.python_version()}")
            exit()


sys.path.append(f"PLUX-API-Python3/{osDic[platform.system()]}")

# Initialize buffer and QRS complex detector with a dummy signal
#dummy_signal = nk.ecg_simulate(duration=2, sampling_rate=500)
#qrs_detector = nk.ecg_qrs_detect(signal=dummy_signal, sampling_rate=500)

import plux

class NewDevice(plux.SignalsDev):
    def __init__(self, address):
        plux.MemoryDev.__init__(address)
        self.duration = 0
        self.frequency = 0

    def onRawFrame(self, nSeq, data):  # onRawFrame takes three arguments
        settings.curr_data = data
        for i in range (len(settings.type)):
            value = settings.curr_data[settings.ports[i]]
            type = settings.type[i]
            if type == 'ECG':
                aux = 2**16
                first = value/aux
                second = first - (1/2)
                third = second * 3.3
                fourth = third / 1100
                final_val = fourth * 1000
                print(type,final_val,settings.hr)
                #print(settings.hr)
                settings.ecg.append(final_val)
                settings.buffer.append(final_val)
                if (len(settings.buffer) >= 10000):
                    aux_clean = nk.ecg_clean(settings.buffer, 500)
                    peaks_dict = nk.ecg_peaks(aux_clean, 500, 'neurokit', False)[1]
                    peaks = peaks_dict['ECG_R_Peaks']
                    heart_rate = nk.ecg_rate(peaks, 500)
                    settings.hr = np.mean(heart_rate)
                    settings.buffer = []
            elif type == 'RESP':
                aux = 2**16
                first = value/(aux-1)
                second = first - (1/2)
                final_val = second * 3
                #print(type,final_val)
                settings.resp.append(final_val)
            else:
                print("sensor nao reconhecido")

# example routines
def exampleAcquisition(
    address="00:07:80:65:E0:24",
    duration=20,
    frequency=500,
    code=0x0F,
):  # time acquisition for each frequency
    """
    Example acquisition.
    Supported channel number codes:
    {1 channel - 0x01, 2 channels - 0x03, 3 channels - 0x07
    4 channels - 0x0F, 5 channels - 0x1F, 6 channels - 0x3F
    7 channels - 0x7F, 8 channels - 0xFF}
    Maximum acquisition frequencies for number of channels:
    1 channel - 8000, 2 channels - 5000, 3 channels - 4000
    4 channels - 3000, 5 channels - 3000, 6 channels - 2000
    7 channels - 2000, 8 channels - 2000
    """
    
    sensorClasses = ['UNKNOWN', 'EMG', 'ECG', 'LUX', 'EDA', 'BVP', 'RESP', 'XYZ', 'SYNC', 'EEG', 'SYNC_ADAP', 'SYNC_LED', 'SYNC_SW', 'USB', 'FORCE', 'TEMP', 'VPROBE', 'BREAKOUT', 'SpO2', 'GONI', 'ACT', 'EOG', 'EGG']

    device = NewDevice(address)
    device.duration = int(duration)  # Duration of acquisition in seconds.
    device.frequency = int(frequency)  # Samples per second.
    if isinstance(code, str):
        code = int(code, 16)  # From hexadecimal str to int

    d = device.getSensors()
    for j in d:
        settings.ports.append(j-1)
        settings.type.append(sensorClasses[d[j].clas])

    device.start(device.frequency, code, 16)
    device.loop()  # calls device.onRawFrame until it returns True
    device.stop()
    device.close()


if __name__ == "__main__":
    # Use arguments from the terminal (if any) as the first arguments and use the remaining default values.
    exampleAcquisition()