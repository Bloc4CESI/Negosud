import React, { useState } from "react";
import "./Login.css";

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
    // Ici, vous pouvez ajouter la logique de connexion, par exemple une requête API
    console.log("Username:", username);
    console.log("Password:", password);
  };

  return (
    <div className="login-container">
      <div className="login-form">
        <h2>Connexion</h2>
        <form>
          <label>
            Mail
            <input type="text" value={username} onChange={handleUsernameChange} />
          </label>
          <br />
          <label>
            Mot de passe
            <input type="password" value={password} onChange={handlePasswordChange} />
          </label>
          <br />
          <button type="button" onClick={handleLogin}>
            Se connecter
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
