-- User can be also be instructor, there is no need for separate role (for now).

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

-- M-to-N
CREATE TABLE tag
(
    id_tag      INT PRIMARY KEY IDENTITY(1, 1),
    tag_title   VARCHAR(10) UNIQUE,
)

-- User
CREATE TABLE [user]
(
    id_user             INT PRIMARY KEY IDENTITY(1, 1),
    first_name          VARCHAR(255), -- JWT claim
    last_name           VARCHAR(255), -- JWT claim
    username            VARCHAR(255) UNIQUE,
    hashed_password     VARCHAR(MAX),
    id_role             INT FOREIGN KEY REFERENCES [role](id_role) -- JWT claim
)

-- Primary
CREATE TABLE course
(
    id_course       INT PRIMARY KEY IDENTITY(1, 1),
    id_user         INT FOREIGN KEY REFERENCES [user](id_user), -- created by user
    id_course_type  INT FOREIGN KEY REFERENCES course_type(id_course_type),
    title           VARCHAR(255) UNIQUE
)

-- Section Type
CREATE TABLE section_type
(
    id_section_type     INT PRIMARY KEY IDENTITY(1, 1),
    section_type        VARCHAR(255) UNIQUE
)

-- M-to-N (Multiple users can read multiple courses)
CREATE TABLE user_course
(
    id_user     INT FOREIGN KEY REFERENCES [user](id_user),
    id_course   INT FOREIGN KEY REFERENCES course(id_course)
)

-- M-to-N
CREATE TABLE course_tag
(
    id_course INT FOREIGN KEY REFERENCES course(id_course),
    id_tag INT FOREIGN KEY REFERENCES tag(id_tag)
)

-- 1-to-N
CREATE TABLE course_section
(
    id_course           INT FOREIGN KEY REFERENCES course(id_course),
    id_section_type     INT FOREIGN KEY REFERENCES section_type(id_section_type),
    content             NVARCHAR(MAX)
)

-- Log
CREATE TABLE log
(
    id_log INT PRIMARY KEY IDENTITY(1, 1),
    content VARCHAR(MAX)
)