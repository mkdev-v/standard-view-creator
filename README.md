# 💡 Standard View Creator for XrmToolBox

This is a plugin for [XrmToolBox](https://www.xrmtoolbox.com/), designed for administrators.  
It enables administrators to create multiple user views across entities in bulk, using standard and commonly used fields.

## ✨ Features

- Create multiple public and personal views in bulk
- Automatically detects and loads entities from the current solution
- Configurable view naming using dynamic tokens
- Customizable column layout (position, order, width)
- Duplicate existing views as public or personal views
- Export views to Excel:
	- Export all views to Excel
	- Export views for selected entities
	- Load an XML file and export its views to Excel

## 🧭 Usage

### 🧩 1. Select Entities
- The tool loads entities included in the current solution.
- Entities with `IsValidForAdvancedFind = false` are excluded, as views cannot be created for them.

### 🛠️ 2. Configure View Settings

- **Type**  
  Supports *User View (Personal View)* and *Public View*.  

- **Name**  
  You can define a dynamic view name using the following tokens:  
  `{!EntityLogicalName}`, `{!EntityDisplayName}`, `{!yyyyMMdd}`  
  Example:  
  `Audit Monitoring ({!EntityLogicalName})` → `Audit Monitoring (account)`

- **Overwrite Handling**  
  Views with the same name are skipped.  

- **Filter Template**  
  Supports 'No Filter', 'Active Only', and 'Inactive Only'.   

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

- Column order (up to 3 positions)
- Sort priority (up to 3 sort levels)
- Column width (up to 300px)

### 📤 4. Export Views

Views can be exported to Excel in three ways:

- **All Views**  
  Export all generated views to Excel.

- **Selected Entities**  
  Export only the views belonging to the entities you selected.

- **From XML File**  
  Load one or multiple XML files and export to Excel.

## 📄 License

This project is licensed under the [MIT License](./LICENSE).

This plugin uses MiniExcel (https://github.com/mini-software/MiniExcel), which is licensed under the Apache License 2.0.