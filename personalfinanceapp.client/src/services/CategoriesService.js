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
}