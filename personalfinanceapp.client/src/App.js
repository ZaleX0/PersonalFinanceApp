import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';

import "primeflex/primeflex.css"
import "primereact/resources/themes/md-dark-indigo/theme.css";
import "primereact/resources/primereact.min.css";                  
import "primeicons/primeicons.css";                                
import { Login } from './pages/Login';
import { Navbar } from './components/Navbar';
import GuardedRoute from './utils/GuardedRoute';
import { AuthProvider } from './contexts/AuthContext';
import { Register } from './pages/Register';
import IncomesExpenses from './pages/IncomesExpenses';
import Categories from './pages/Categories';

export default function App() {
  return (
    <AuthProvider>
      <div className="m-auto" style={{maxWidth: "960px"}}>
        <Navbar />
        <DefineRoutes />
      </div>
    </AuthProvider>
  )
}

function DefineRoutes() {
  return (
    <>
      <Routes>
        <Route path="/login" element={<Login/>}/>
        <Route path="/register" element={<Register/>}/>
        <Route path="/" element={<GuardedRoute><Home/></GuardedRoute>}/>
        <Route path="/incomes-expenses" element={<GuardedRoute><IncomesExpenses/></GuardedRoute>}/>
        <Route path="/categories" element={<GuardedRoute><Categories/></GuardedRoute>}/>
      </Routes>
    </>
  )
}