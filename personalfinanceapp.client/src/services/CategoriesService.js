import { authHeader } from "../utils/authHeader";

export default class CategoriesService {

  async getIncomeCategories() {
    let url = "/api/incomes/categories";
    return await fetch(url, {
      method: "GET",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
  async getExpenseCategories() {
    let url = "/api/expenses/categories";
    return await fetch(url, {
      method: "GET",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
  async addIncomeCategories(category) {
    let url = "/api/incomes/categories";
    return await fetch(url, {
      method: "POST",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        id: 0,
        name: category.name
      })
    });
  }
  async addExpenseCategories(category) {
    let url = "/api/expenses/categories";
    return await fetch(url, {
      method: "POST",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        id: 0,
        name: category.name
      })
    });
  }
  async udpateIncomeCategories(category) {
    let url = `/api/incomes/categories/${category.id}`;
    return await fetch(url, {
      method: "PUT",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        id: category.id,
        name: category.name
      })
    });
  }
  async udpateExpenseCategories(category) {
    let url = `/api/expenses/categories/${category.id}`;
    return await fetch(url, {
      method: "PUT",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        id: category.id,
        name: category.name
      })
    });
  }
  async deleteIncomeCategories(id) {
    let url = `/api/incomes/categories/${id}`;
    return await fetch(url, {
      method: "DELETE",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
  async deleteExpenseCategories(id) {
    let url = `/api/expenses/categories/${id}`;
    return await fetch(url, {
      method: "DELETE",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
}