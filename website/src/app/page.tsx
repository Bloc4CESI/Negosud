import Header from "../../modules/layout/header";
import "./globals.css";
import Footer from "../../modules/layout/footer";
import ImgIllustration1 from "../../public/images/vin_accueil.jpg";
import ImgIllustration2 from "../../public/images/femme_rose.png";

export default function Home() {
  return (
    <>
      <Header />
      <div className="container mx-auto p-12 my-12 shadow-lg shadow-gray-300 rounded-xl">
      <div className="flex flex-col md:flex-row items-center">
        <div className="md:w-1/2 mb-4 md:mb-0">
          <p className="text-lg md:text-xl mb-4">
            Bienvenue chez NegoSud, votre partenaire de confiance pour l'achat de vins en ligne.
          </p>
          <p className="text-lg md:text-xl mb-4">
            Découvrez notre histoire et notre passion pour les vins de qualité.
          </p>
          <a href="./listingProducts" type="button" className="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center items-center inline-flex me-2 mb-2">
            Découvrir
            <svg className="rtl:rotate-180 w-3.5 h-3.5 ms-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 10">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M1 5h12m0 0L9 1m4 4L9 9"/>
            </svg>
          </a>
        </div>
        <div className="md:w-1/2 md:ml-8">
            <img src={ImgIllustration1.src} alt="Description de l'image" className="rounded-full" />
        </div>
      </div>

      <div className="flex flex-col md:flex-row items-center mt-8">
        <div className="md:w-1/2 order-2 md:order-1 md:mr-8">
            <img src={ImgIllustration2.src} alt="Description de l'image" className="rounded-full" />
        </div>
        <div className="md:w-1/2 order-1 md:order-2">
          <p className="text-lg md:text-xl mb-4">
            Explorez nos sélections de vins exceptionnels, soigneusement choisis pour votre plaisir.
          </p>
          <p className="text-lg md:text-xl mb-4">
            Chez NegoSud, nous nous engageons à vous offrir une expérience de dégustation unique.
          </p>
          <p className="text-lg md:text-xl mb-4">
            Nos fournisseur sont choisis avec une précision comparable à de l'élitisme.
            <br/>
            Tandis-que nos vignerons sont une parfaite représentation du capitalisme.
          </p>
        </div>
      </div>
    </div>
      <Footer />
    </>
  )
}
