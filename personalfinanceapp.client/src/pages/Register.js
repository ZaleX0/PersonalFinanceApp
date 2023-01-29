import { InputText } from 'primereact/inputtext'
import { Password } from 'primereact/password'
import { Card } from 'primereact/card';
import { useFormik } from 'formik';
import { useEffect, useState } from 'react';
import { classNames } from 'primereact/utils';
import { Button } from 'primereact/button';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router';
import { Link } from 'react-router-dom';
import AuthService from "../services/AuthService";

export function Register() {
  const { isLogin } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isLogin) {
      navigate("/");
    }
  }, [])

  const [showErrorMessage, setShowErrorMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const authService = new AuthService()
  const formik = useFormik({
    initialValues: {
      username: '',
      password: '',
      confirmPassword: ''
    },
    validate: (data) => { 
      let errors = {};
      if (!data.username) errors.username = 'Username is required.';
      if (!data.password) errors.password = 'Password is required.';
      if (data.username.length < 3) errors.username = 'Username must be at least 3 characters';
      if (data.password.length < 6) errors.password = 'Password must be at least 6 characters';
      if (data.password !== data.confirmPassword) errors.confirmPassword = "Passwords are not the same."
      return errors;
    },
    onSubmit: (data) => {
      setShowErrorMessage("");
      registerUser(data);
    }
  });

  const registerUser = async (data) => {
    setLoading(true);
    const response = await authService.register(data.username, data.password);
    if (response.status === 200) {
      navigate("/")
    } else if (response.status === 400) {
      const json = await response.json();
      if (json.errors.Username) {
        setShowErrorMessage(json.errors.Username[0]);
      }
    }
    setLoading(false);
  }

  const isFormFieldValid = (name) => !!(formik.touched[name] && formik.errors[name]);
  const getFormErrorMessage = (name) => {
    return isFormFieldValid(name) && <small className="p-error">{formik.errors[name]}</small>;
  };

  return (
    <div className="flex justify-content-center">
      <Card className="text-center pl-4 pr-4">
        <h2>Register</h2>
        Have an account? <Link to="/login">Sign in</Link>
        <form onSubmit={formik.handleSubmit} className="p-fluid mt-4">
          <div className="field">
            <span className="p-float-label">
              <InputText id="username" name="username" value={formik.values.username} onChange={formik.handleChange} autoFocus className={classNames({ 'p-invalid': isFormFieldValid('username') })} />
              <label htmlFor="username" className={classNames({ 'p-error': isFormFieldValid('username') })}>Username</label>
            </span>
            {getFormErrorMessage('username')}
          </div>
          <div className="field">
            <span className="p-float-label">
              <Password id="password" name="password" value={formik.values.password} onChange={formik.handleChange} toggleMask feedback={false} className={classNames({ 'p-invalid': isFormFieldValid('password') })} />
              <label htmlFor="password" className={classNames({ 'p-error': isFormFieldValid('password') })}>Password</label>
            </span>
            {getFormErrorMessage('password')}
          </div>
          <div className="field">
            <span className="p-float-label">
              <Password id="confirmPassword" name="confirmPassword" value={formik.values.confirmPassword} onChange={formik.handleChange} feedback={false} className={classNames({ 'p-invalid': isFormFieldValid('confirmPassword') })} />
              <label htmlFor="confirmPassword" className={classNames({ 'p-error': isFormFieldValid('confirmPassword') })}>Confirm password</label>
            </span>
            {getFormErrorMessage('confirmPassword')}
          </div>
          <p className="text-center p-error">{showErrorMessage}</p>
          <Button type="submit" label="Submit" className="mt-4" loading={loading}/>
        </form>
      </Card>
    </div>
  )
}