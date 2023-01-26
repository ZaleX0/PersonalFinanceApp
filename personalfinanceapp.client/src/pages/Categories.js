import { Card } from 'primereact/card'
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import React, { useEffect, useState } from 'react'
import CategoriesService from '../services/CategoriesService'
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Field, Form, Formik } from 'formik';
import { fireEvent } from '@testing-library/react';
import { Dialog } from 'primereact/dialog';

export default function Categories() {
  const categoriesService = new CategoriesService();

  const [incomeCategories, setIncomeCategories] = useState([]);
  const [expenseCategories, setExpenseCategories] = useState([]);
  const [selecetedCategory, setSelecetedCategory] = useState({});
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);

  const fetchData  = async () => {
    categoriesService.getIncomeCategories()
      .then(response => response.json())
      .then(data => setIncomeCategories(data));
    categoriesService.getExpenseCategories()
      .then(response => response.json())
      .then(data => setExpenseCategories(data));
  }

  useEffect(() => {
    fetchData();
  }, [])

  const openDeleteDialog = (rowData, incomeExpenseType) => {
    rowData = {...rowData, type: incomeExpenseType}
    setSelecetedCategory(rowData);
    setShowDeleteDialog(true);
  }

  const incomeHeader = () => <h2 className="text-center">Income Categories</h2>
  const expenseHeader = () => <h2 className="text-center">Expense Categories</h2>
  const textEditor = (options) => <InputText type="text" value={options.value} onChange={(e) => options.editorCallback(e.target.value)} className="w-full"/>
  const onCellEditComplete = (event, incomeExpenseType) => {
    let { rowData, value, newValue, field } = event;
    newValue = newValue.trim();
    if (newValue === null || newValue === '' || value === newValue) {
      return;
    }
    rowData[field] = newValue;
    incomeExpenseType === 0
      ? categoriesService.udpateIncomeCategories(rowData)
      : categoriesService.udpateExpenseCategories(rowData)
  }

  function addNewCategoryForm(incomeExpenseType) {
    return (
      <Formik
        initialValues={{name: ''}}
        validate={values => {
          const errors = {};
          if (!values.name) errors.name = "Required";
          return errors;
        }}
        validateOnMount
        onSubmit={async (values, actions) => {
          actions.resetForm();
          incomeExpenseType === 0
            ? await categoriesService.addIncomeCategories(values)
            : await categoriesService.addExpenseCategories(values);
          fetchData();
        }}
      >
        {(props) => 
          <Form>
            <div className="p-inputgroup">
              <Field name="name">
                {({field}) => 
                  <span className="p-float-label">
                    <InputText {...field}/>
                    <label>Add new category</label>
                  </span>
                }
              </Field>
              <Button type="submit" icon="pi pi-plus" disabled={!props.isValid }/>
            </div>
          </Form>
        }
      </Formik>
    )
  }

  function deleteDialog() {
    const hide = () => setShowDeleteDialog(false);
    const handleDelete = async () => {
      hide();
      if (selecetedCategory.type === 0) {
        categoriesService.deleteIncomeCategories(selecetedCategory.id);
        let _incomeCategories = incomeCategories.filter(val => val.id !== selecetedCategory.id)
        setIncomeCategories(_incomeCategories);
      } else {
        categoriesService.deleteExpenseCategories(selecetedCategory.id);
        let _expenseCategories = expenseCategories.filter(val => val.id !== selecetedCategory.id)
        setExpenseCategories(_expenseCategories);
      }
    }
    const footer = <div>
      <Button label="Yes" onClick={handleDelete} className="p-button-text"/>
      <Button label="No" onClick={hide} autoFocus/>
    </div>

    return (
      <Dialog visible={showDeleteDialog} onHide={hide} header="Delete" footer={footer}>
        Do you want to delete this category?
      </Dialog>
    )
  }

  const deleteButtonBody = (rowData, incomeExpenseType) => {
    return (
      <div className="flex justify-content-end">
        <Button icon="pi pi-trash" className="p-button-rounded p-button-danger" onClick={()=>openDeleteDialog(rowData, incomeExpenseType)}/>
      </div>
    )
  }

  return (
    <>
      <div className="grid p-2 gap-2">
        <Card header={incomeHeader} className="col">
          {addNewCategoryForm(0)}
          <DataTable value={incomeCategories} editMode="cell">
            <Column field="name" editor={textEditor} onCellEditComplete={e=>onCellEditComplete(e, 0)}/>
            <Column body={rowData=>deleteButtonBody(rowData, 0)}/>
          </DataTable>
        </Card>
        <Card header={expenseHeader} className="col">
          {addNewCategoryForm(1)}
          <DataTable value={expenseCategories}>
            <Column field="name" editor={textEditor} onCellEditComplete={e=>onCellEditComplete(e, 1)}/>
            <Column body={rowData=>deleteButtonBody(rowData, 1)}/>
          </DataTable>
        </Card>
      </div>
      {deleteDialog()}
    </>
  )
}