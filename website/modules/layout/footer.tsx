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

  const social = [
    {route : "https://www.facebook.com/", svg : <Facebook className="h-6"/>},
    {route : "https://www.instagram.com/", svg : <Instagram className="h-6"/>},
    {route : "https://www.x.com/", svg : <Twitter className="h-6"/>},
    {route : "https://www.linkedin.com/", svg : <Linkedin className="h-6"/>},
  ];

  return (
    <div className="flex flex-col items-center justify-center w-full bg-black">
      <ul className="flex flex-row justify-evenly w-full mt-3">
        {footer.map((item, index) => (
          <li key={index} className="text-sm text-white">
            <a href={item.route}>{item.titre}</a>
          </li>
        ))}
      </ul>
      <div className="flex flex-row items-center justify-center w-full mt-4">
        <p className="text-sm text-white">L&apos;abus d&apos;alcool est dangereux pour la santé, a consommer avec modération.</p>
      </div>
      <ul className="flex flex-row w-full justify-center mt-4 gap-3">
        {social.map((item, index) => (
          <li key={index}>
            <a href={item.route}>{item.svg}</a>
          </li>
        ))}
      </ul>
      <div className="flex flex-row items-center justify-center w-full mt-4 mb-4">
        <p className="text-sm text-white">© 2024 Negosud. Tous droits réservés.</p>
      </div>
    </div>
  );
};

export default Footer;
