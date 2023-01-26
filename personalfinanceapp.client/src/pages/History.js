import { ErrorMessage, Field, Form, Formik } from 'formik';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { InputText } from 'primereact/inputtext';
import { InputNumber } from 'primereact/inputnumber';
import { Calendar } from 'primereact/calendar';
import { Column } from 'primereact/column';
import { Dropdown } from 'primereact/dropdown';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { useEffect, useState } from 'react';
import IncomeExpenseService from '../services/IncomeExpenseService';
import formatDate from '../utils/formatDate';
import CategoriesService from '../services/CategoriesService';

export default function History() {
  const incomeExpenseService = new IncomeExpenseService();
  const categoriesService = new CategoriesService();
  const [incomesExpenses, setIncomesExpenses] = useState([]);
  const [incomeCategories, setIncomeCategories] = useState([]);
  const [expenseCategories, setExpenseCategories] = useState([]);
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const [showEditDialog, setShowEditDialog] = useState(false);
  const [selectedIncomeExpense, setSelectedIncomeExpense] = useState({});
  const [query, setQuery] = useState({
    search: "",
    incomeCategoryId: "",
    expenseCategoryId: "",
    dateFrom: "",
    dateTo: ""
  });

  useEffect(() => {
    const fetchData = async () => {
      const response = await incomeExpenseService.getIncomesExpenses("");
      const json = await response.json();
      json.forEach(element => element.date = formatDate(element.date));
      setIncomesExpenses(json);

      categoriesService.getIncomeCategories()
        .then(response => response.json())
        .then(data => setIncomeCategories(data));
      categoriesService.getExpenseCategories()
        .then(response => response.json())
        .then(data => setExpenseCategories(data));
    }
    fetchData();
  }, [])
  
  const openDeleteDialog = (rowData) => {
    setSelectedIncomeExpense(rowData);
    setShowDeleteDialog(true);
  }
  const openEditDialog = (rowData) => {
    setSelectedIncomeExpense(rowData);
    setShowEditDialog(true);
  }

  const priceBodyTemplate = (rowData) => {
    return rowData.type === 0
      ? <div className="text-green-400">+{rowData.price}</div>
      : <div className="text-red-400">-{rowData.price}</div>
  }

  const actionBodyTemplate = (rowData) => {
    return (
      <div className="flex justify-content-end">
        <Button icon="pi pi-pencil" className="p-button-rounded p-button-info mr-2" onClick={()=>openEditDialog(rowData)}/>
        <Button icon="pi pi-trash" className="p-button-rounded p-button-danger" onClick={()=>openDeleteDialog(rowData)}/>
      </div>
    )
  }

  function deleteDialog() {
    const hide = () => setShowDeleteDialog(false);
    const handleDelete = async () => {
      await incomeExpenseService.deleteIncomeExpense(selectedIncomeExpense);
      let _incomeExpense = incomesExpenses.filter(val => val.id !== selectedIncomeExpense.id || val.type !== selectedIncomeExpense.type)
      setIncomesExpenses(_incomeExpense);
      hide();
    }
    const footer = <div>
      <Button label="Yes" onClick={handleDelete} className="p-button-text"/>
      <Button label="No" onClick={hide} autoFocus/>
    </div>

    return (
      <Dialog visible={showDeleteDialog} onHide={hide} header="Delete" footer={footer}>
        Do you want to delete this {selectedIncomeExpense.type === 0 ? "income" : "expense"}?
      </Dialog>
    )
  }

  function getCategoryName(type, categoryId) {
    const categories = type === 0 ? incomeCategories : expenseCategories;
    const category = categories.find(c => c.id === categoryId);
    return category.name;
  }

  function editDialog() {
    const hide = () => setShowEditDialog(false);
    const footer = <div>
      <Button label="Cancel" onClick={hide} className="p-button-text"/>
      <Button label="Submit" onClick={hide}/>
    </div>

    return (
      <Dialog visible={showEditDialog} onHide={hide} header={selectedIncomeExpense.type === 0 ? "Edit income" : "Edit expense"}>
        <Card>
          <Formik
            initialValues={{
              categoryId: selectedIncomeExpense.categoryId,
              price: selectedIncomeExpense.price,
              comment: selectedIncomeExpense.comment,
              date: new Date(selectedIncomeExpense.date)
            }}
            validate={values => {
              const errors = {};
              if (!values.date) errors.date = "Required";
              if (!values.price) errors.price = "Required";
              return errors;
            }}
            onSubmit={async (data) => {
              data.id = selectedIncomeExpense.id;
              data.type = selectedIncomeExpense.type;
              const newIncomesExpenses = incomesExpenses.map(ie => {
                if (ie.id === data.id && ie.type === data.type)
                  return {
                    id: ie.id,
                    type: ie.type,
                    categoryId: data.categoryId,
                    categoryName: getCategoryName(ie.type, data.categoryId),
                    price: data.price,
                    comment: data.comment,
                    date: formatDate(data.date)
                  };
                return ie;
              });
              setIncomesExpenses(newIncomesExpenses);
              data.date = formatDate(data.date)
              hide();
              await incomeExpenseService.updateIncomeExpense(data);
            }}
          >
            {(props) => (
              <div className="flex justify-content-center">
              <Form className="p-fluid">
                <Field name="price">
                  {({ field }) => (
                    <span className="p-float-label">
                      <InputNumber
                        autoFocus
                        mode="decimal"
                        maxFractionDigits={2}
                        value={field.value}
                        onChange={e => props.setFieldValue("price", e.value)}
                        onBlur={props.onBlur}
                        className={props.errors.price && 'p-invalid'}
                      />
                      <label htmlFor="price" className={props.errors.price && 'p-error'}>Price</label>
                    </span>
                  )}
                </Field>
                <Field name="categoryId">
                  {({ field }) => {
                    const options = selectedIncomeExpense.type === 0
                      ? incomeCategories
                      : expenseCategories;
                    return (
                      <span className="p-float-label mt-4">
                        <Dropdown {...field} options={options} optionLabel="name" optionValue="id"/>
                        <label htmlFor="date">Category</label>
                      </span>
                    )}
                  }
                </Field>
                <Field name="date">
                  {({ field }) => (
                    <span className="p-float-label mt-4">
                      <Calendar {...field} dateFormat="yy-mm-dd" className={props.errors.date && 'p-invalid'} showIcon/>
                      <label htmlFor="date" className={props.errors.date && 'p-error'}>Date</label>
                    </span>
                  )}
                </Field>
                <Field name="comment">
                  {({ field }) => (
                    <span className="p-float-label mt-4">
                      <InputText {...field}/>
                      <label htmlFor="comment">Comment</label>
                    </span>
                  )}
                </Field>
                <Button type="submit" label="Update" className="mt-4"/>
              </Form>
              </div>
            )}
          </Formik>
        </Card>
      </Dialog>
    )
  }

  return (
    <>
      <Card>
        <DataTable value={incomesExpenses} size="small" paginator rows={10} stripedRows className="surface-border border-x-1 border-top-1">
          <Column field="date" header="Date"/>
          <Column field="price" header="Price" body={priceBodyTemplate}/>
          <Column field="categoryName" header="Category"/>
          <Column field="comment" header="Comment"/>
          <Column body={actionBodyTemplate}/>
        </DataTable>
      </Card>
      {editDialog()}
      {deleteDialog()}
    </>
  )
}