<!-- PROJECT LOGO -->
<div align="center">

# QubitX

  <p align="center">
    QubitX is an online web application that offers various courses and tutorials primarily related to programming and other IT topics in general. 
  </p>
</div>

&nbsp;

<!-- ABOUT THE PROJECT -->
## About The Project

![qubitx-preview](https://github.com/user-attachments/assets/e3a69795-1041-454a-91b8-255b0504bdba)


QubitX is an advanced online web application designed to provide a seamless learning experience through a wide variety of programming and IT-related courses. 
The platform serves as a comprehensive resource for both beginners and advanced users, offering structured tutorials and lessons that cover essential IT topics,
software development practices, and various programming languages.

**Key Features:**
- Course Creation and Customization: QubitX empowers users to create fully customized courses and tutorials using either Markdown syntax or regular HTML,
- providing flexibility and ease in content creation. The app leverages highlight.js for syntax highlighting, making code snippets more readable and engaging, while marked.js ensures smooth Markdown parsing.
- Interactive Learning Environment: Users can follow courses interactively, engaging with programming exercises and learning materials with syntax highlighting and easy-to-read code blocks.
- The dynamic content display makes it easier to understand complex programming concepts.
- Admin Panel: A robust admin panel is provided for administrators to manage the platform efficiently. Admins can create, edit, or remove courses, track user activities,
- and log changes to ensure the platform is up-to-date. This allows for easy moderation and management of content.

&nbsp;

## Built With

### Main Languages & Frameworks
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![.Net](https://img.shields.io/badge/ASP.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

### Database & API
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![.Net](https://img.shields.io/badge/EF-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

### Design & Basic Functionalities
![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)
![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)

### Other Frameworks
![JavaScript](https://img.shields.io/badge/Highlight.js-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)
![JavaScript](https://img.shields.io/badge/markedjs-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)

&nbsp;

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

To use make this application work you need to install Docker and pull SQLServer image and create container. You can do that by following next steps according to your system.

&nbsp;

#### Windows

1. Go to docker website: https://www.docker.com/
2. Choose one of the download options according to your system (AMD64, ARM64, etc.) and follow instructions on installer.
3. When installation is finished, pull sql server image from [Docker HUB](https://hub.docker.com/r/microsoft/mssql-server)
4. Run this image for the first time to create an instance of sqlserver using command:

  ```sh
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 --name your-container-name -d mcr.microsoft.com/mssql/server
  ```

> [!TIP]
> Change MSSQL_SA_PASSWORD and --name values accordingly.

&nbsp;

#### Linux
1. Go to docker website: https://www.docker.com/
2. Choose one of the download options according to your chosen distribution and follow instruction on docker website.
3. Pull the sql server image from [Docker HUB](https://hub.docker.com/r/microsoft/mssql-server)
4. Run this image for the first time to create an instance of sqlserver using command:

  ```sh
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 --name your-container-name -d mcr.microsoft.com/mssql/server
  ```

> [!TIP]
> Change MSSQL_SA_PASSWORD and --name values accordingly.

&nbsp;

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/TomislavS23/QubitX.git
   ```
3. Open solution in either Jetbrains Rider or Visual Studio 2022
4. In the solution, open WebAPI module and double-click on appsettings.json
5. Under **ConnectionStrings** modify **server** connection string with password you used for container creation. Other variables can stay the same.
6. Connect to your sqlserver instance with DB management system of your choice. I recommend using **Jetbrains DataGrip** or **VSCode** with SQLServer extensions.

> [!IMPORTANT]
> In both programs you connect to localhost. Username should be "**sa**" by default and password should be the password you configured on first start of sqlserver instance.
>
7. Create database "qubitx" on your SQLServer instance.
  ```sql
  CREATE DATABASE qubitx
  ```
8. Go back to cloned project and enter "Database" directory.
9. Run Database.sql script on created database.
10. You are all set! Now you need to run WebAPI module and WebAPP module at the same time and enjoy your working QubitX application!

&nbsp;

<!-- USAGE EXAMPLES -->
## Usage

QubitX is designed for simplicity and ease of use, catering to learners, course creators, and administrators.

**For Learners:**
- Browse and Enroll: Explore courses by topic or search for specific ones. Enroll with a click to access course materials.
- Interactive Learning: Lessons feature highlighted code snippets using highlight.js and structured content via Markdown/HTML.
- Track Progress: Mark lessons as complete and monitor your progress through the course.
- Support & Feedback: Reach out for help or leave feedback to improve course quality.

**For Course Creators:**
- Create Courses: Use Markdown or HTML to build interactive courses with highlighted code blocks and rich content.
- Organize Content: Structure your course into sections and modules for easy navigation.
- Publish & Manage: Publish courses for learners, and update content or monitor learner feedback as needed.

**For Administrators:**
- Manage Courses: Admins can create, edit, or remove courses and moderate content.
- Track Changes: Log and review updates made to courses or user activity.
- User Management: Admins can manage users and assign roles like learner, creator, or admin.

QubitX offers a streamlined experience, making it easy to learn, create, and manage content effectively.

&nbsp;

> [!TIP]
> For more information (and general learning) about frameworks and languages in this project, please refer to the next resources:
> 
> [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/)
> 
> [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
> 
> [Entity Framework Core](https://learn.microsoft.com/en-us/ef/)
>
> 

&nbsp;

<!-- CONTRIBUTING -->
## Contributing
- Project is under MIT licence, please read carefully what you can or can't do with this project.
- Project is currently not active. That means that it will not be actively maintained by me or anyone else for timebeing.

&nbsp;

<!-- CONTACT -->

<!--
## Contact

name - handle for something - email@email_client.com

Project Link: //

<p align="right">(<a href="#readme-top">back to top</a>)</p>
-->
