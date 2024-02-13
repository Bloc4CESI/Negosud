"use client";
import React from "react";
import Facebook from "../../public/img/facebook.svg";
import Linkedin from "../../public/img/linkedin.svg";
import Instagram from "../../public/img/instagram.svg";
import Twitter from "../../public/img/twitter.svg";

const Footer: React.FC = () => {
  const footer = [
    {titre : "Nous contacter", route : "/contact"},
    {titre : "Mentions légales", route: "/legals"},
    {titre : "CGU", route: "/legals"},
    {titre : "A propos", route: "/about"},
    {titre : "Nous rejoindre", route: "/join"},
  ];

  return (
    <div className="flex flex-col bottom-0 items-center justify-center w-full bg-gray-900 p-4 lg:p-8">
      <ul className="flex flex-row justify-evenly w-full">
        {footer.map((item, index) => (
          <li key={index} className="text-sm text-white">
            <a href={item.route}>{item.titre}</a>
          </li>
        ))}
      </ul>
      <div className="flex flex-row items-center justify-center w-full mt-4">
        <p className="text-sm text-white">L&apos;abus d&apos;alcool est dangereux pour la santé, à consommer avec modération.</p>
      </div>
      <div className="flex flex-row items-center justify-center w-full mt-4">
        <p className="text-sm text-white">© 2024 Negosud. Tous droits réservés.</p>
      </div>
    </div>
  );

};

export default Footer;
