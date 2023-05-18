# Q-analysis 

The Q-analysis is an implementation for performing Q-analysis. This program allows users to analyze data and calculate various quality parameters based on the Q-analysis methodology. It uses an open-source plotting library [ScottPlot](https://scottplot.net/) for data visualization.

## Features

- Import data from CSV file format.
- Perform Q-analysis calculations on the imported data.
- Generate visualizations such as histograms.

## Installation

To use this program, download the installer from the [Releases](https://github.com/Ivruix/AnalyticHierarchyProcess/releases) page. After downloading the installer, run it and follow the on-screen instructions to install the program on your computer.

or

1. Clone the repository or download the program as a ZIP file.
2. Open the project in your preferred development environment (e.g., Visual Studio).
3. Build the solution to restore dependencies and compile the program.

### Main Menu
1. Create a new project.
2. Open previously created projects.


### New Project Window
1. Binary matrix.
2. Weighted matrix.


### Matrix Input Window
1. Matrix input functionality.
2. Input of valid range for matrix elements.
3. Choose a scalar slice value or a matrix of slices. For the matrix of slices, a separate window is opened where the user can load or manually enter the slice matrix, which should match the size of the original matrix.

### Results Window
1. The results window include the Q-vector (a list indicating all the connected component elements defined in the analysis), simplex eccentricities (a list), and the matrix entered by the user.
2. Visualize the results.
3. Choose simplex eccentricities according to Casti J.L. or Duckstein L.
4. Save the results.


### Visualization Window
1. View of the visualization of the results.
2. View of detailed expanded Q-vector.


### Slice Procedure Window
1. The slice procedure window contains a button to switch between scalar slice value and slice matrix.
2. Ability to return to the results window.
3. Window displaying the obtained results after applying the slice. The window allows expanding the Q-vector to see its components.


### Compare Window
1. Choose the scalar slice range.
2. Compare Q-analysis procedure results. The comparison window displays two Q-vectors selected for comparison. 
   - Green color indicates differing-sized simplices. 
   - Orange color indicates simplices of the same size but with different components.


## Usage

1. Launch the Q-analysis program.
2. Select the data file you want to analyze or manually enter the data.
3. Perform the desired Q-analysis calculations and choose the parameters to analyze.
4. Explore the generated visualizations to gain insights into the data.
5. Export the analysis results and visualizations for reporting or further analysis.

## Requirements

- .NET Framework 6.0 or higher

## Screenshots

| Main Menu | New Project Creation | Settings Window |
| :--------:| :-----------------: | :-------------: |
| ![Main Menu](https://github.com/DeadRick/Q-analysis/assets/39325834/1ca3f979-329c-4c93-9332-81be144411d6) | ![New Project Creation](https://github.com/DeadRick/Q-analysis/assets/39325834/e4c03032-f647-4d9e-a839-9431b4ac37ef) | ![Settings Window](https://github.com/DeadRick/Q-analysis/assets/39325834/b67af330-8235-411a-b22e-22f80fc0bf53) |

| Result Window | Compare Window | Visualization |
| :-----------: | :------------: | :-----------: |
| ![Result Window](https://github.com/DeadRick/Q-analysis/assets/39325834/dafcaeb8-0dee-4245-ba1f-f35ccebe722e) | ![Compare Window](https://github.com/DeadRick/Q-analysis/assets/39325834/a8741321-df6e-400b-af81-5afc27725ce7) | ![Visualization](https://github.com/DeadRick/Q-analysis/assets/39325834/eb858b72-4bfc-4bf7-9e8d-804eccb9634a) |


## Contributing

If you find a bug or have a feature request, please open an issue on the project's GitHub repository. Pull requests are also welcome!

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

