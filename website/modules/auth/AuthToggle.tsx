import React, { useState } from "react";

interface AuthToggleProps {
  onToggle: () => void;
  isLogin: boolean;
  text: string;
  className?: string;
}

const AuthToggle: React.FC<AuthToggleProps> = ({ onToggle, text, className }) => {
  const [isClicked, setIsClicked] = useState(false);

  const handleClick = () => {
    setIsClicked((prevIsClicked) => !prevIsClicked);
    onToggle();
  };

  return (
    <span className={className} onClick={handleClick}>
      {text}
    </span>
  );
};

export default AuthToggle;
