"use client";
import "../globals.css";
import Footer from "../../../modules/layout/footer";
import Header from "../../../modules/layout/header";
export default function About() {
  return (
    <>
    <Header/>
    <div className="max-w-3xl mx-auto p-6 bg-white shadow-md my-16 rounded-md">
      <h1 className="text-3xl font-bold mb-6">À Propos de NégoSud Vin</h1>
        <section className="mb-8">
          <p className="text-lg text-gray-700">Bienvenue chez NégoSud Vin, votre destination en ligne pour découvrir et acheter
            les meilleurs vins du Sud. Nous sommes passionnés par l&apos;art du vin et nous nous engageons à vous
            fournir une sélection exceptionnelle de vins de qualité, soigneusement choisis pour satisfaire les
            palais les plus exigeants.</p>
        </section>
        <section className="mb-8">
          <h2 className="text-xl font-semibold mb-4">Notre Mission :</h2>
          <p className="text-gray-700">À NégoSud Vin, notre mission est de vous offrir une expérience unique en vous
            proposant des vins exceptionnels qui racontent une histoire, captivent vos sens et enrichissent vos
            moments spéciaux. Nous croyons que le vin va au-delà d&apos;une simple boisson - c&apos;est une passion partagée
            et une invitation à explorer le monde fascinant des saveurs.</p>
        </section>
        <section className="mb-8">
          <h2 className="text-xl font-semibold mb-4">Notre Sélection :</h2>
          <p className="text-gray-700">Chaque bouteille que nous proposons chez NégoSud Vin est le résultat d&apos;une sélection
            rigoureuse. Nous travaillons en étroite collaboration avec des producteurs passionnés du Sud de la
            France pour vous offrir des vins uniques, caractéristiques de la richesse et de la diversité de cette
            région viticole exceptionnelle.</p>
        </section>
        <section className="mb-8">
          <h2 className="text-xl font-semibold mb-4">Contactez-Nous :</h2>
          <p className="text-gray-700">Pour toute question ou information supplémentaire, n&apos;hésitez pas à nous contacter. Nous
            sommes ravis de partager notre passion pour le vin avec vous.</p>
          <p className="text-gray-700">Adresse e-mail : <a href="mailto:info@negosudvin.com">info@negosudvin.com</a></p>
          <p className="text-gray-700">Téléphone : <a href="tel:+123456789">+123 456 789</a></p>
        </section>
    </div>
  <Footer/>
</>
)}