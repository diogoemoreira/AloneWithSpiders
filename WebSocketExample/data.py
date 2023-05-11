from unicodedata import name
import settings
import sample

from datetime import datetime, timezone

name_ecg = datetime.now(timezone.utc).strftime("%d-%m-%Y %H_%M_%S_ecg.txt")
name_resp = datetime.now(timezone.utc).strftime("%d-%m-%Y %H_%M_%S_resp.txt")

settings.init()

try:
    while True:
        sample.exampleAcquisition()

except KeyboardInterrupt:
    with open(name_ecg, 'a') as f:
        for item in settings.ecg:
            f.write("%s\n" % str(item))
        f.close()

    with open(name_resp, 'a') as f:
        for item in settings.resp:
            f.write("%s\n" % str(item))
        f.close()