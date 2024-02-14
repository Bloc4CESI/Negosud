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
    <Header style="bg-white stroke-black"/>
    <div className="flex flex-col items-center">
    {isChecked ?  <Login handleToggle={handleToggle} isChecked={isChecked} /> :
      <Register handleToggle={handleToggle} isChecked={isChecked}/>}
    </div>
    <Footer/>
  </>
  )
};
