// Home.js
import React from 'react';
import Image from 'next/image';
import wineImage from '../images/vin.png';
import Header from '../../../modules/layout/header';
import Footer from "../../../modules/layout/footer";

const Home = () => {
  return (
    <div>
      <Header />

      <div className="container mx-auto mt-8">
        <header className="text-center">
          <h1 className="text-4xl font-bold mb-4">Bienvenue sur NegoSud</h1>
          <p className="text-lg text-gray-600">Découvrez une sélection exceptionnelle de vins.</p>
        </header>

        <section className="mt-12">
          {/* Utilisation de la balise Image de Next.js */}
          <Image src={wineImage} alt="Wine Bottles" className="rounded-lg shadow-lg mx-auto" height={200} width={200} />
        </section>

        <section className="mt-8 text-center">
          <h2 className="text-2xl font-semibold mb-4">Derniers Arrivages</h2>
          {/* Ajoutez ici une liste de vos derniers vins avec des images et des descriptions */}
        </section>

        <section className="mt-8 text-center">
          <h2 className="text-2xl font-semibold mb-4">Nos Catégories</h2>
          {/* Ajoutez ici une liste de vos catégories de vins avec des liens vers les pages correspondantes */}
        </section>

        <section className="mt-8 text-center">
          <h2 className="text-2xl font-semibold mb-4">À Propos de Nous</h2>
          <p className="text-lg text-gray-600">
            Découvrez notre passion pour le vin et notre engagement envers la qualité.
          </p>
        </section>
      </div>
      <Footer/>
    </div>
  );
};

export default Home;
