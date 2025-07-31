# 💡 Standard View Creator for XrmToolBox

This is a plugin for [XrmToolBox](https://www.xrmtoolbox.com/), designed for administrators.  
It allows you to bulk-create user views across multiple entities, using standard and commonly referenced fields.

## ✨ Features

- Bulk creation of personal views (User Views)
- Automatically selects entities from the current solution
- Configurable view naming using dynamic tokens
- Customizable column layout (order, sorting, width)

## 🧭 Usage

### 🧩 1. Select Entities
- The tool loads entities included in the current solution.
- Entities with `IsValidForAdvancedFind = false` are excluded, as views cannot be created for them.

### 🛠️ 2. Configure View Settings

- **Type**  
  Currently supports only *User View (Personal View)*.  
  Support for *System View* is planned in future updates.

- **Name**  
  You can define a dynamic view name using the following tokens:  
  `{entityLogicalName}`, `{entityDisplayName}`, `{yyyyMMdd}`  
  Example:  
  `Audit Monitoring ({entityLogicalName})` → `Audit Monitoring (account)`

- **Duplicate Handling**  
  Currently, views are skipped if a view with the same name exists.  
  Overwrite options are planned in a future release.

- **Filter Template**  
  Currently, only "No Filter" is supported.  
  Templates such as "Active Only" and "Inactive Only" are under development.

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