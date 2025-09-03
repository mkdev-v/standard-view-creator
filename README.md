# 💡 Standard View Creator for XrmToolBox

This is a plugin for [XrmToolBox](https://www.xrmtoolbox.com/), designed for administrators.  
It allows you to bulk-create user views across multiple entities, using standard and commonly referenced fields.

## ✨ Features

- Bulk creation of public/personal views
- Automatically selects entities from the current solution
- Configurable view naming using dynamic tokens
- Customizable column layout (position, order, width)
- Export the list of views to Excel
- Duplicate a View (as public/personal view)

## 🧭 Usage

### 🧩 1. Select Entities
- The tool loads entities included in the current solution.
- Entities with `IsValidForAdvancedFind = false` are excluded, as views cannot be created for them.

### 🛠️ 2. Configure View Settings

- **Type**  
  Currently supports *User View (Personal View)* and *Public View*.  

- **Name**  
  You can define a dynamic view name using the following tokens:  
  `{entityLogicalName}`, `{entityDisplayName}`, `{yyyyMMdd}`  
  Example:  
  `Audit Monitoring ({entityLogicalName})` → `Audit Monitoring (account)`

- **Overwrite Handling**  
  Currently, views are skipped if a view with the same name exists.  

- **Filter Template**  
  Currently supports "No Filter" and "Active Only", "Inactive Only".  

### 🧮 3. Configure Columns

The following columns are available for inclusion in the view:

- `createdon`
- `createdby`
- `createdonbehalfby`
- `modifiedon`
- `modifiedby`
- `modifiedonbehalfby`
- `name` → The primary field of the entity
- `statecode`
- `statuscode`
- `ownerid`
- `owningbusinessunit`

Users can customize:

- Column order (up to the top 3 positions)
- Sort priority (up to the top 3)
- Column width (maximum: 300px)

## 📄 License

This project is licensed under the [MIT License](./LICENSE).