import React from 'react';
import { Navigate } from "react-router-dom";
import { useAuth } from '../hooks/useAuth';

function GuardedRoute({ children }) {
  const { isLogin } = useAuth();
  if (!isLogin) {
    return <Navigate to="/login" replace />
  }
  return children;
}

export default GuardedRoute;
