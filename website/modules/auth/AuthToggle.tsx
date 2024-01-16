import React, { useState } from "react";

interface AuthToggleProps {
  onToggle: () => void;
  isLogin: boolean;
}

const AuthToggle: React.FC<AuthToggleProps> = ({ onToggle }) => {
  const [isClicked, setIsClicked] = useState(false);

  const handleClick = () => {
    setIsClicked((prevIsClicked) => !prevIsClicked);
    onToggle();
  };

  return (
    <div
      onClick={handleClick}
      className="border w-fit rounded-xl m-5 shadow-sm"
    >
      <button
        className={`px-4 py-2 rounded-l-xl  ${
          isClicked ? 'bg-white text-black' : 'bg-red-500 text-white'
        } transition-colors duration-300`}
      >
        Connexion
      </button>
      <button
        className={`px-4 py-2 rounded-r-xl ${
          isClicked ? 'bg-red-500 text-white' : 'bg-white text-black'
        } transition-colors duration-300`}
      >
        Inscription
      </button>
    </div>
  );
};

export default AuthToggle;
