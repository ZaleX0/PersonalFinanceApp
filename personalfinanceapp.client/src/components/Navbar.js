import React, { useState } from 'react'
import { Menubar } from "primereact/menubar"
import { useNavigate } from 'react-router';
import { useAuth } from '../hooks/useAuth';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';

export function Navbar() {
  const {user, logout, isLogin} = useAuth();
  const [showLogoutDialog, setShowLogoutDialog] = useState(false);
  const navigate = useNavigate();

  const items = [
    {label: 'Home', icon: "pi pi-fw pi-home", command:()=>navigate("/")}
  ];

  const end = isLogin
  && <div className="flex align-items-center">
      <p><span className="text-400">Signed in as</span> {user.username}</p>
      <Button label="Logout" onClick={()=>setShowLogoutDialog(true)} className="ml-4"/>
    </div>

  return (
    <>
      <Menubar model={items} className="mb-4 mt-4" end={end} />
      {logoutDialog()}
    </>
  )

  function logoutDialog() {
    const hide = () => setShowLogoutDialog(false);

    const footer = <div>
      <Button label="No" onClick={hide} className="p-button-text"/>
      <Button label="Yes" onClick={()=>{hide(); logout()}} autoFocus/>
    </div>

    return (
      <Dialog visible={showLogoutDialog} onHide={hide} footer={footer} closable={false}>
        <div className="text-center">Are you sure?</div>
      </Dialog>
    )
  }
}