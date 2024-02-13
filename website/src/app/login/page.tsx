"use client"
import Login from "../../../modules/auth/login";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import { useState } from "react"
import '../globals.css';
import Register from "../../../modules/auth/register";
import AuthToggle from "../../../modules/auth/AuthToggle";

export default function LoginPage()  {
  const [isChecked, setIsChecekd] = useState(true);
  const handleToggle = () => {
    setIsChecekd((prevIsChecked) => !prevIsChecked);
  };

  return (
  <>
    <Header style={undefined}/>
    <div className="flex flex-col items-center">
    <AuthToggle onToggle={handleToggle} isLogin={isChecked} />
    {isChecked ?  <Login/> : <Register/>}
    </div>
    <Footer/>
  </>
  )
};
