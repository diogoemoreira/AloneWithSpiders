import dash
import dash_core_components as dcc
import dash_html_components as html
import os
import numpy as np
import plotly.graph_objs as go
import neurokit2 as nk

datae = [] #data for ecg
datar = [] #data for respiratory
datahr = [] #data for HR

directory = os.getcwd()  # Get the current working directory
text_files = []
for filename in os.listdir(directory):
    if filename.endswith(".txt"):
        text_files.append(filename)

for file in text_files:
    if "ecg" in file:
        # Read data from a text file
        with open(file, "r") as file:
            lines = file.readlines()
            datae = [float(line.strip()) for line in lines]
            
            aux_clean = nk.ecg_clean(datae, 500)
            peaks_dict = nk.ecg_peaks(aux_clean, 500, 'neurokit', False)[1]
            peaks = peaks_dict['ECG_R_Peaks']
            heart_rate = nk.ecg_rate(peaks, 500)
            datahr = [np.mean(hr) for hr in heart_rate]
    else:
        # Read data from a text file
        with open(file, "r") as file:
            lines = file.readlines()
            datar = [line.strip() for line in lines]

time = np.arange(len(datae)) / 500
time_hr = np.linspace(0, len(datae) / 500, len(heart_rate))

# Create the Dash application
app = dash.Dash(__name__)

# Define the layout of the dashboard
app.layout = html.Div(
    children=[
        html.H1("Dashboard"),
        dcc.Graph(
            id="ecg-graph",
            figure={
                "data": [
                    go.Scatter(
                        x=time,
                        y=datae,
                        mode="lines",
                        name="ECG Graph"
                    )
                ],
                "layout": go.Layout(
                    title="ECG Graph",
                    xaxis={"title": "Time (s)"},
                    yaxis={"title": "ECG (mV)"}
                )
            },
        ),
        dcc.Graph(
            id="heart-rate-graph",
            figure={
                "data": [
                    go.Scatter(
                        x=time_hr,
                        y=datahr,
                        mode="lines",
                        name="Heart Rate Graph"
                    )
                ],
                "layout": go.Layout(
                    title="Heart Rate Graph",
                    xaxis={"title": "Time (s)"},
                    yaxis={"title": "Heart Rate (bpm)"}
                )
            },
        ),
        dcc.Graph(
            id="resp-graph",
            figure={
                "data": [
                    go.Scatter(
                        x=time,
                        y=datar,
                        mode="lines",
                        name="Respiration Graph"
                    )
                ],
                "layout": go.Layout(
                    title="Respiration Graph",
                    xaxis={"title": "Time (s)"},
                    yaxis={"title": "Respiration (V)"}
                )
            },
        ),
    ]
)

# Run the application
if __name__ == "__main__":
    app.run_server(debug=True)