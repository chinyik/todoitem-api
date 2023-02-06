# SleekFlow - TodoItem CRUD API
Simple REST API to perform  CRUD operations on TodoItem model.

## Tools used
- .NET Core 7
- MySql Server 8
- NUnit Framework

## Database Schema
**Table Name:** TodoItems

| Column        | DataType                  |
| ------------  | ------------              |
| todoItemId    | INT (PK, AUTO-INCREMENT)  |
| name          | VARCHAR(45)               |
| description   | VARCHAR(200)              |
| duedate       | DATETIME                  |
| status        | VARCHAR(45)               |

## API List
### ~/api/todoitems
- Fetch all TodoItems either with filtering or sorting

| Name               | Description                                                    | DataType     |
| ------------       | ------------                                                   | ------------ |
| nameFilter         | Filter TodoItems by name column                                | string       |
| descriptionFilter  | Filter TodoItems by description column                         | string       |
| dueDateFilter      | Filter TodoItems by due date column                            | datetime     |
| statusFilter       | Filter TodoItems by status column                              | string       |
| sortBy             | Specify which column name to sort TodoItems                    | string       |
| isDescending       | Specify sort order (default false => ascending) for TodoItems  | bool         |

- Return all TodoItems (or some of the TodoItems that satisfy the filtering conditions)

### [GET] ~/api/todoitems/{id}
Fetch 1 TodoItem
- Required parameter:
	- **id**: id of todoitem to be fetched
- Return created TodoItem

### [POST] ~/api/todoitems
Add 1 TodoItem
- Sample request:

```json
POST api/TodoItems
{
   "name": "Item #1",
   "description": "Description #1",
   "dueDate": "2023-02-06",
   "status": "todo"
}
```

### [PUT] ~/api/todoitems/{id}
Update 1 TodoItem of matching id
- Required parameter:
	- **id**: id of todoitem to be updated
	- Sample request:

```json
PUT api/TodoItems/{id}
{
   "todoItemId": {id},
   "name": "Item #1",
   "description": "Description #1",
   "dueDate": "2023-02-06",
   "status": "todo"
}
```

### [DELETE] ~/api/todoitems/{id}
Delete 1 TodoItem of matching id
- Required parameter:
	- **id**: id of todoitem to be deleted
