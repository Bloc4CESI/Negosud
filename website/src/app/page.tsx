"use client"
import Header from "../../modules/layout/header";
import "./globals.css";
import Login from "./auth/login";
import Footer from "../../modules/layout/footer";

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
