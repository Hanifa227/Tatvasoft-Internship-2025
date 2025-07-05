
-- 1. DATABASE & TABLE CREATION

-- Q1: Create a database named 'practice_db'
-- Explanation: This creates a new database to store your tables and data.
CREATE DATABASE practice_db;

-- Q2: Create 'departments' table
-- Explanation: This table stores department names and has a unique ID.
CREATE TABLE departments (
    department_id SERIAL PRIMARY KEY,
    department_name VARCHAR(100) UNIQUE NOT NULL
);

-- Q3: Create 'employees' table
-- Explanation: This table stores employee information including their department.
CREATE TABLE employees (
    employee_id SERIAL PRIMARY KEY,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    email VARCHAR(150) UNIQUE NOT NULL,
    department VARCHAR(100),
    salary NUMERIC(10, 2),
    hire_date TIMESTAMPTZ DEFAULT NOW()
);

-- Q4: Add foreign key
-- Explanation: Ensures 'department' in employees matches a valid department name.
ALTER TABLE employees
ADD CONSTRAINT fk_department FOREIGN KEY (department)
REFERENCES departments(department_name);


-- 2. INSERTING & MODIFYING RECORDS

-- Q5: Insert records into departments
-- Explanation: Add sample department entries.
INSERT INTO departments (department_name) VALUES
('IT'), ('HR'), ('Finance'), ('Sales'), ('Marketing');

-- Q6: Insert employee data
-- Explanation: Insert 10 records with valid department values.
INSERT INTO employees (first_name, last_name, email, department, salary)
VALUES
('John', 'Doe', 'john.doe@example.com', 'IT', 60000),
('Alice', 'Smith', 'alice.smith@example.com', 'HR', 55000),
('Bob', 'Johnson', 'bob.johnson@example.com', 'Finance', 70000),
('Emma', 'Brown', 'emma.brown@example.com', 'IT', 80000),
('David', 'Clark', 'david.clark@example.com', 'Sales', 65000),
('Olivia', 'Martinez', 'olivia.martinez@example.com', 'HR', 53000),
('James', 'Garcia', 'james.garcia@example.com', 'Marketing', 62000),
('Sophia', 'Lopez', 'sophia.lopez@example.com', 'Sales', 68000),
('Michael', 'Lee', 'michael.lee@example.com', 'IT', 72000),
('Sarah', 'Wilson', 'sarah.wilson@example.com', 'Finance', 75000);

-- Q7: Add a new column 'active'
-- Explanation: Adds a boolean flag to show active employees.
ALTER TABLE employees ADD COLUMN active BOOLEAN DEFAULT true;

-- Q8: Update employee salary
-- Explanation: Modify the salary for employee with ID 1.
UPDATE employees SET salary = 90000 WHERE employee_id = 1;

-- Q9: Delete an employee by email
-- Explanation: Remove employee with the specified email.
DELETE FROM employees WHERE email = 'test@example.com';

-- =======================================
-- 3. SELECT & BASIC QUERY PRACTICE
-- =======================================
-- Q10: Show all employees
SELECT * FROM employees;
SELECT * FROM departments;


-- Q11: Employees from IT department
SELECT * FROM employees WHERE department = 'IT';

-- Q12: Employees earning more than 50000
SELECT first_name, salary FROM employees WHERE salary > 50000;

-- Q13: List employees by last name descending
SELECT * FROM employees ORDER BY last_name DESC;

-- Q14: First names with letter 'a' (case-insensitive)
SELECT * FROM employees WHERE first_name ILIKE '%a%';

-- Q15: Show unique departments from employee table
SELECT DISTINCT department FROM employees;

-- Q16: Top 5 employees by salary
SELECT * FROM employees ORDER BY salary DESC LIMIT 5;

-- =======================================
-- 4. JOIN PRACTICE
-- =======================================
-- Q17: Join employees with department names
-- Explanation: Match department name using INNER JOIN.
SELECT e.first_name, e.last_name, d.department_name
FROM employees e
JOIN departments d ON e.department = d.department_name;

-- =======================================
-- 5. GROUPING & AGGREGATION
-- =======================================
-- Q18: Count employees per department
SELECT department, COUNT(*) AS total_employees
FROM employees
GROUP BY department;

-- Q19: Average salary per department
SELECT department, AVG(salary) AS avg_salary
FROM employees
GROUP BY department;

-- Q20: Departments where total salary > 200000
SELECT department, SUM(salary) AS total_salary
FROM employees
GROUP BY department;
-- HAVING SUM(salary) > 200000;

-- =======================================
-- 6. SUBQUERY PRACTICE
-- =======================================
-- Q21: Employees from departments with more than 2 employees
SELECT * FROM employees
WHERE department IN (
    SELECT department FROM employees
    GROUP BY department
    HAVING COUNT(*) > 2
);

-- Q22: Employee(s) with the highest salary
SELECT * FROM employees
WHERE salary = (SELECT MAX(salary) FROM employees);

-- Q23: Employees with duplicate salaries
SELECT * FROM employees e1
WHERE EXISTS (
    SELECT 1 FROM employees e2
    WHERE e1.salary = e2.salary AND e1.employee_id <> e2.employee_id
);

-- =======================================
-- 7. EXTRA CHALLENGES
-- =======================================
-- Q24: Rename column salary to monthly_salary and back
ALTER TABLE employees RENAME COLUMN salary TO monthly_salary;
ALTER TABLE employees RENAME COLUMN monthly_salary TO salary;

-- Q25: Drop 'active' column
ALTER TABLE employees DROP COLUMN active;

-- Q26: Drop employees table only if it exists
DROP TABLE IF EXISTS employees;


--left join
SELECT e.first_name, e.last_name, d.department_name
FROM employees e
LEFT JOIN departments d ON e.department = d.department_name;

--right join
SELECT d.department_name, e.first_name
FROM employees e
RIGHT JOIN departments d ON e.department = d.department_name;

--full outer join
SELECT e.first_name, d.department_name
FROM employees e
FULL JOIN departments d ON e.department = d.department_name;
