"use client"
import React, { useState } from "react";
import "../globals.css";
import Header from "../../../modules/layout/header";

const Login: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUsername(e.target.value);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
  };

  const handleLogin = () => {
    console.log("Username:", username);
    console.log("Password:", password);
  };
// <div className="text-white h-[100vh] flex justify-center items-center bg-cover">
return (
    <>
    <Header/>
    <div className="container">
    <div className="h-[100vh] flex justify-center items-center bg-cover">
        <div className="col-md-6 bg-blue-400 rounded p-8">
            <h1>Bienvenue chez <strong>NegoSud</strong></h1>
            <form>
                <div className="form-group">
                <label htmlFor="username">Email de l'utilisateur</label>
                <input
                    type="email"
                    className="form-control"
                    id="username"
                    name="email"
                    placeholder="Entrez votre nom d'utilisateur"
                />
                </div>
                <div className="form-group">
                <label htmlFor="password">Mot de passe</label>
                <input
                    type="password"
                    className="form-control"
                    id="password"
                    name="password"
                    placeholder="Entrez votre mot de passe"
                />
                </div>
                <div className="form-group text-center">
                <button type="submit" id="submit" className="btn btn-primary">Se connecter</button>
                </div>
            </form>
        </div>
    </div>
    </div>
    </>
);
};

export default Login;
