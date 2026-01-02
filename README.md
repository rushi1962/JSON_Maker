📋 JSON Maker (Unity)
=

A designer-friendly extension tool to make JSON data files that allows non-technical people to create JSON text files from data classes with custom attribute, within Unity itself.

✨ Overview
=

JSON Maker is a custom Unity editor tool that lets users create ready to use JSON text files from from data classes with custom attribute. Using JSON Maker tool, users can:
- Convert a list of class elements (with 'JSONConvertable' attributes) into a JSON text file.
- Load a JSON text file and have those elements shown in the JSON maker window.


🎯 Key Goals
=

- Providing a better experience to users to create a JSON file within Unity.
- Even non-technical people should be able to use and make JSON files without headache of using third party websites/softwares.


🔧 Features
=

JSON class selection window
-
- Shows the list of all the serializable classes with 'JSONConvertable' attribute and allows you to select any class from those classes.

JSON Maker window
-
- Allows user to create a new JSON text file of selected class at mentioned path.
- Allow user to load existing JSON file at the mentioned path.


👀 Glimps At The Tool Inside Unity
=
<img width="546" height="728" alt="image" src="https://github.com/user-attachments/assets/e8b9a2c6-4225-41cf-8152-30d90da97cee" />


<img width="607" height="772" alt="image" src="https://github.com/user-attachments/assets/249bd3b1-2280-4beb-b197-9311603b341c" />


<img width="602" height="760" alt="image" src="https://github.com/user-attachments/assets/8b748ec5-5bb8-47d6-98ef-b9856da1e67a" />


📂 Project Structure
=

<img width="1010" height="434" alt="image" src="https://github.com/user-attachments/assets/dcbd27a7-70a3-4dac-83ef-6c8675f8a4c9" />



🚀 Getting Started
=

1. Clone or download the repository
3. Open the project in Unity (Unity version: 2022.3.31f1 or above)
4. Go to window->JSON Maker Window
5. Select the class for which you want to make JSON file.
6. Create a new file or load existing file at the given path.
7. Add/remove elements and populate the list.
8. Save the JSON file once the list is ready.
9. You can add data classes in the class selection list but adding attribute 'JSONConvertable' to the classes.


⚠️ Known Limitations
=

Unity's existing JSONUtility functions cannot convert lists into a JSON string. So a way around to this was to convert each element into the list into the JSON and 
club them in one single string with custom formatting. JSON string with this format can only be retrieved by JSONUtilityLibray in this project and not by Unity's default JSONUtility.

🎓 Why This Exists
=

This project was built to help non -technical people to create JSON files within Unity project without having to go to any third party website.

🙌 Author
=
Rushikesh Charapale

Senior Gameplay / Tools Developer

🔗 GitHub: <[rushi1962](https://github.com/rushi1962)>

🔗 LinkedIn: <[Rushikesh Charapale](https://www.linkedin.com/in/rushikesh-charapale-389288178/)>
