"use client"
import Header from "../../modules/layout/header";
import "./globals.css";
import Login from "./login/page";
import Footer from "../../modules/layout/footer";
import { useAuth } from "../../services/api/user/useAuth";
import { useEffect } from "react";

export default function Home() {

  return (
    <>
   <Header/>
    <div>
      Page d'accueil
    </div>
    <Footer/>
    </>
  )
}
