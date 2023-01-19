import { InputText } from 'primereact/inputtext'
import { Password } from 'primereact/password'
import { Card } from 'primereact/card';
import { useFormik } from 'formik';
import { useState } from 'react';
import { classNames } from 'primereact/utils';
import { Button } from 'primereact/button';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router';
import { Link } from 'react-router-dom';

export function Register() {
  const { login, isLogin } = useAuth();
  const [showErrorMessage, setShowErrorMessage] = useState(false);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  if (isLogin) {
    navigate("/");
  }

  const fetchUser = async (data) => {
    setLoading(true);
    const response = await login(data.username, data.password);
    if (response.status === 200) {
      navigate("/")
    } else {
      setShowErrorMessage(true);
    }
    setLoading(false);
  }

  const formik = useFormik({
    initialValues: {
      username: '',
      password: ''
    },
    validate: (data) => {
      let errors = {};
      if (!data.username) errors.username = 'Username is required.';
      if (!data.password) errors.password = 'Password is required.';
      return errors;
    },
    onSubmit: (data) => {
      setShowErrorMessage(false);
      formik.resetForm();
      fetchUser(data);
    }
  });

  const isFormFieldValid = (name) => !!(formik.touched[name] && formik.errors[name]);
  const getFormErrorMessage = (name) => {
    return isFormFieldValid(name) && <small className="p-error">{formik.errors[name]}</small>;
  };

  return (
    <div className="flex justify-content-center">
      <Card className="text-center pl-4 pr-4">
        <h2>Login</h2>
        <Link to="/register">Sign up</Link>
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
          {showErrorMessage && <p className="text-center p-error">Invalid username or password</p>}
          <Button type="submit" label="Submit" className="mt-4" loading={loading}/>
        </form>
      </Card>
    </div>
  )
}