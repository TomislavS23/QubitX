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
    id_course_type  INT PRIMARY KEY IDENTITY(1, 1),
    course_type     VARCHAR(255) UNIQUE
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

INSERT INTO course_type(course_type)
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
('jon', 'snow', 'user', '04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb', 1),
('eddard', 'stark', 'admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 2)