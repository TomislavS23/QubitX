-- User can be also be an instructor by default, there is no need for separate role (for now).
-- dotnet ef dbcontext scaffold "Name=ConnectionStrings:server" Microsoft.EntityFrameworkCore.SqlServer -c QubitXContext -o Models -f

-- 1-to-N
CREATE TABLE [role]
(
    id_role     INT PRIMARY KEY IDENTITY(1, 1),
    role_type   VARCHAR(50)
)

-- 1-to-N
CREATE TABLE course_type
(
    id_course_type      INT PRIMARY KEY IDENTITY(1, 1),
    course_type_title   VARCHAR(255) UNIQUE
)

-- Tag
CREATE TABLE tag
(
    id_tag      INT PRIMARY KEY IDENTITY(1, 1),
    tag_title   VARCHAR(50) UNIQUE,
)

-- User
CREATE TABLE [user]
(
    id_user             INT PRIMARY KEY IDENTITY(1, 1),
    first_name          VARCHAR(255),
    last_name           VARCHAR(255),
    username            VARCHAR(255) UNIQUE, -- JWT claim
    hashed_password     VARCHAR(MAX),
    id_role             INT FOREIGN KEY REFERENCES [role](id_role) -- JWT claim
)

-- Course (PRIMARY)
CREATE TABLE course
(
    id_course       INT PRIMARY KEY IDENTITY(1, 1),
    id_user         INT FOREIGN KEY REFERENCES [user](id_user), -- created by user
    id_course_type  INT FOREIGN KEY REFERENCES course_type(id_course_type),
    course_title    VARCHAR(255) UNIQUE,
    course_content  NVARCHAR(MAX)
)

-- M-to-N (Multiple users can read multiple courses)
CREATE TABLE user_course
(
    id_user_course  INT PRIMARY KEY IDENTITY(1, 1),
    id_user         INT FOREIGN KEY REFERENCES [user](id_user),
    id_course       INT FOREIGN KEY REFERENCES course(id_course)
)

-- M-to-N
CREATE TABLE course_tag
(
    id_course_tag INT PRIMARY KEY IDENTITY(1, 1),
    id_course INT FOREIGN KEY REFERENCES course(id_course),
    id_tag INT FOREIGN KEY REFERENCES tag(id_tag)
)

-- Log
CREATE TABLE log
(
    id_log          INT PRIMARY KEY IDENTITY(1, 1),
    log_timestamp   DATETIME,
    log_level       TINYINT CHECK(log_level > 0 AND log_level <= 5),
    log_message     VARCHAR(MAX)
)

/* ========================================================== */
/* ======================= INSERTS ========================== */
/* ========================================================== */

INSERT INTO role(role_type)
VALUES ('User'),
       ('Admin')

INSERT INTO course_type(course_type_title)
VALUES ('Computer Science'),
       ('Programming'),
       ('Operating Systems'),
       ('Low Level Programming'),
       ('Video game development'),
       ('Web Development'),
       ('Android Development'),
       ('Desktop Development'),
       ('Artificial Intelligence')

INSERT INTO tag(tag_title)
VALUES ('C#'), ('ASP.NET'), ('EF'), ('C'),
       ('C++'), ('Java'), ('Kotlin'), ('HTML'),
       ('CSS'), ('JS'), ('Bootstrap CSS'), ('React'),
       ('Node'), ('Angular'), ('Verilog'), ('Assembly'),
       ('XAML'), ('XML'), ('JSON'), ('YML'), ('Markdown'),
       ('SQL'), ('PostgreSQL'), ('MongoDB'), ('T-SQL'), ('MSSQL'),
       ('Git'), ('GitHub'), ('Console'), ('Scripting'),
       ('ML'), ('DL'), ('DRL'), ('RL'),
       ('LLM'), ('CV'), ('Robotics'), ('NN'),
       ('NLP'), ('EC'), ('APS'), ('Cognitive Computing'),
       ('Python'), ('Julia'), ('Lisp'), ('Prolog'),
       ('Scala'), ('SWIFT'), ('LAN'), ('WLAN'), ('SoC')


INSERT INTO [user](first_name, last_name, username, hashed_password, id_role) VALUES
('jon', 'snow', 'user', '04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb', 1), -- PASS: user
('eddard', 'stark', 'admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 2) -- PASS: admin

INSERT INTO course (id_user, id_course_type, course_title, course_content) VALUES
(1, 1, 'Introduction to Programming', 'In this course, you will learn the fundamentals of programming including basic concepts such as variables, data types, control structures, and algorithms. The course is designed for beginners and will help you build a strong foundation in programming. Practical exercises and examples will guide you through the learning process.'),
(2, 2, 'Advanced Data Structures', 'This course covers complex data structures such as trees, graphs, and hash tables. You will explore advanced topics like balancing trees, graph algorithms, and their applications in real-world scenarios. Understanding these structures will enhance your problem-solving skills and improve the efficiency of your code.'),
(1, 3, 'Web Development Basics', 'Learn the essentials of web development, including HTML, CSS, and JavaScript. This course will guide you through creating your first web pages and styling them effectively. You will also be introduced to responsive design principles and basic front-end frameworks. Perfect for those new to web development.'),
(2, 4, 'Machine Learning 101', 'Get started with machine learning in this introductory course. Topics include supervised and unsupervised learning, model evaluation, and basic algorithms such as linear regression and clustering. Hands-on projects and examples will help you apply the concepts to real-world problems.'),
(1, 5, 'Database Management Systems', 'This course provides an overview of database management systems (DBMS), including SQL, relational databases, and normalization techniques. You will learn to design, implement, and manage databases effectively. Practical exercises will include creating queries, optimizing performance, and understanding transactions.'),
(2, 6, 'Network Security Fundamentals', 'Explore the basics of network security, including common threats, security policies, and protective measures. Topics covered include firewalls, encryption, VPNs, and intrusion detection systems. This course will help you understand the essential concepts to safeguard networks from potential attacks.'),
(1, 7, 'Operating Systems Overview', 'Gain a comprehensive understanding of operating systems including their design, functionality, and key components. You will learn about process management, memory management, and file systems. The course will also cover concepts such as multitasking and system calls.'),
(2, 8, 'Introduction to Algorithms', 'This course introduces fundamental algorithms and their applications. Topics include sorting, searching, and algorithmic complexity. You will learn to analyze the efficiency of algorithms and implement them to solve various computational problems.'),
(1, 9, 'Cloud Computing Essentials', 'Learn the basics of cloud computing, including cloud service models (IaaS, PaaS, SaaS), deployment models, and major cloud providers. This course will cover how to leverage cloud services for scalable and cost-effective solutions, as well as best practices for cloud security and management.'),
(2, 1, 'Modern JavaScript Techniques', 'This course covers modern JavaScript features and techniques, including ES6+ syntax, asynchronous programming, and advanced functions. You will learn to write clean, efficient, and maintainable JavaScript code and explore popular frameworks and libraries.'),
(1, 2, 'Introduction to Data Science', 'Get introduced to data science, including data analysis, visualization, and statistical methods. You will learn how to use tools such as Python and R for data manipulation and gain insights from data sets. Practical projects will help you apply these skills to real-world scenarios.'),
(2, 3, 'Building RESTful APIs', 'Learn how to design and build RESTful APIs using modern frameworks. This course covers API architecture, endpoints, HTTP methods, and authentication. You will also explore best practices for API documentation and versioning, and how to integrate APIs with front-end applications.'),
(1, 4, 'Cybersecurity Principles', 'Explore the key principles of cybersecurity including risk management, threat assessment, and defensive strategies. This course will cover various aspects of cybersecurity, including secure coding practices, network security, and incident response. Practical exercises will reinforce the concepts learned.'),
(2, 5, 'Big Data Technologies', 'Dive into big data technologies and tools such as Hadoop, Spark, and NoSQL databases. You will learn how to handle and analyze large data sets, perform data processing, and leverage big data solutions to gain insights and drive business decisions.'),
(1, 6, 'Software Engineering Best Practices', 'This course focuses on best practices in software engineering, including software development methodologies, design patterns, and code quality. You will learn to implement effective development processes, write maintainable code, and manage software projects efficiently.'),
(2, 7, 'Human-Computer Interaction', 'Study the principles of human-computer interaction (HCI) to design user-friendly interfaces and improve user experience. This course covers usability testing, user-centered design, and interaction techniques. You will learn to apply HCI principles to create intuitive and effective digital products.'),
(1, 8, 'Game Development Basics', 'Get introduced to game development concepts including game design, mechanics, and programming. You will learn to use game development tools and engines to create simple games. The course covers basic 2D and 3D game development techniques and provides hands-on projects.'),
(2, 9, 'Introduction to Artificial Intelligence', 'Explore the fundamentals of artificial intelligence (AI) including machine learning, neural networks, and natural language processing. This course covers the theoretical and practical aspects of AI and provides hands-on experience with AI tools and technologies.'),
(1, 1, 'Ethical Hacking Techniques', 'Learn ethical hacking techniques to identify and address security vulnerabilities. Topics include penetration testing, vulnerability assessment, and ethical hacking tools. This course will help you understand how to secure systems and networks against potential threats.'),
(2, 2, 'Introduction to Blockchain', 'Discover the fundamentals of blockchain technology, including how it works, its applications, and its impact on various industries. You will learn about decentralized ledgers, smart contracts, and cryptocurrency, and explore the potential uses of blockchain beyond digital currencies.'),
(1, 3, 'Virtual Reality Programming', 'Get introduced to virtual reality (VR) programming and development. This course covers VR concepts, tools, and techniques for creating immersive experiences. You will learn to develop VR applications using popular VR platforms and programming languages.'),
(2, 4, 'Introduction to Cloud Services', 'Explore various cloud services including computing, storage, and databases. This course provides an overview of how cloud services work, how to choose the right service for your needs, and best practices for cloud resource management and optimization.'),
(1, 5, 'Software Testing and Quality Assurance', 'Learn software testing techniques and quality assurance practices to ensure the reliability and performance of software applications. Topics include test planning, test case design, automated testing tools, and defect management. This course will help you understand how to deliver high-quality software.'),
(2, 6, 'Data Visualization Techniques', 'Explore techniques for visualizing data effectively using tools such as Tableau and D3.js. This course covers principles of data visualization, design best practices, and how to create interactive and insightful visualizations to communicate data-driven insights.'),
(1, 7, 'Advanced Web Development', 'Dive deeper into web development with advanced topics including single-page applications, progressive web apps, and modern front-end frameworks. This course will help you build sophisticated web applications and understand complex web technologies.'),
(2, 8, 'Introduction to DevOps', 'Learn the principles and practices of DevOps, including continuous integration, continuous deployment, and infrastructure as code. This course covers the DevOps lifecycle, tools, and strategies for improving collaboration between development and operations teams.'),
(1, 9, 'Introduction to Quantum Computing', 'Explore the basics of quantum computing, including quantum bits, quantum gates, and quantum algorithms. This course provides an overview of quantum computing principles and potential applications, and introduces tools and frameworks for quantum programming.');

-- Course 1: 'Introduction to Programming'
INSERT INTO course_tag (id_course, id_tag) VALUES (1, 8); -- HTML
INSERT INTO course_tag (id_course, id_tag) VALUES (1, 9); -- CSS
INSERT INTO course_tag (id_course, id_tag) VALUES (1, 10); -- JS

-- Course 2: 'Advanced Data Structures'
INSERT INTO course_tag (id_course, id_tag) VALUES (2, 4); -- C++
INSERT INTO course_tag (id_course, id_tag) VALUES (2, 21); -- SQL

-- Course 3: 'Web Development Basics'
INSERT INTO course_tag (id_course, id_tag) VALUES (3, 8); -- HTML
INSERT INTO course_tag (id_course, id_tag) VALUES (3, 9); -- CSS
INSERT INTO course_tag (id_course, id_tag) VALUES (3, 10); -- JS

-- Course 4: 'Machine Learning 101'
INSERT INTO course_tag (id_course, id_tag) VALUES (4, 27); -- ML
INSERT INTO course_tag (id_course, id_tag) VALUES (4, 29); -- NN

-- Course 5: 'Database Management Systems'
INSERT INTO course_tag (id_course, id_tag) VALUES (5, 21); -- SQL
INSERT INTO course_tag (id_course, id_tag) VALUES (5, 22); -- PostgreSQL
INSERT INTO course_tag (id_course, id_tag) VALUES (5, 23); -- MongoDB

-- Course 6: 'Network Security Fundamentals'
INSERT INTO course_tag (id_course, id_tag) VALUES (6, 24); -- T-SQL
INSERT INTO course_tag (id_course, id_tag) VALUES (6, 30); -- Git

-- Course 7: 'Operating Systems Overview'
INSERT INTO course_tag (id_course, id_tag) VALUES (7, 1); -- C#

-- Course 8: 'Introduction to Algorithms'
INSERT INTO course_tag (id_course, id_tag) VALUES (8, 4); -- C++

-- Course 9: 'Cloud Computing Essentials'
INSERT INTO course_tag (id_course, id_tag) VALUES (9, 33); -- Cloud

-- Course 10: 'Modern JavaScript Techniques'
INSERT INTO course_tag (id_course, id_tag) VALUES (10, 10); -- JS

-- Course 11: 'Introduction to Data Science'
INSERT INTO course_tag (id_course, id_tag) VALUES (11, 27); -- ML
INSERT INTO course_tag (id_course, id_tag) VALUES (11, 32); -- Python

-- Course 12: 'Building RESTful APIs'
INSERT INTO course_tag (id_course, id_tag) VALUES (12, 10); -- JS
INSERT INTO course_tag (id_course, id_tag) VALUES (12, 30); -- Git

-- Course 13: 'Cybersecurity Principles'
INSERT INTO course_tag (id_course, id_tag) VALUES (13, 26); -- Security

-- Course 14: 'Big Data Technologies'
INSERT INTO course_tag (id_course, id_tag) VALUES (14, 21); -- SQL
INSERT INTO course_tag (id_course, id_tag) VALUES (14, 22); -- PostgreSQL
INSERT INTO course_tag (id_course, id_tag) VALUES (14, 23); -- MongoDB

-- Course 15: 'Software Engineering Best Practices'
INSERT INTO course_tag (id_course, id_tag) VALUES (15, 1); -- C#
INSERT INTO course_tag (id_course, id_tag) VALUES (15, 4); -- C++

-- Course 16: 'Human-Computer Interaction'
INSERT INTO course_tag (id_course, id_tag) VALUES (16, 8); -- HTML
INSERT INTO course_tag (id_course, id_tag) VALUES (16, 9); -- CSS

-- Course 17: 'Game Development Basics'
INSERT INTO course_tag (id_course, id_tag) VALUES (17, 4); -- C++
INSERT INTO course_tag (id_course, id_tag) VALUES (17, 11); -- Unreal Engine

-- Course 18: 'Introduction to Quantum Computing'
INSERT INTO course_tag (id_course, id_tag) VALUES (18, 31); -- Quantum Computing
