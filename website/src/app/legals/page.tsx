import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import "../globals.css";

export default function Legals() {
  return (
    <>
    <Header />
    <div className="max-w-3xl mx-auto p-6 bg-white shadow-md my-16 rounded-md">
      <h1 className="text-3xl font-bold mb-6">Mentions Légales</h1>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Éditeur du site :</h2>
        <p>NégoSud<br />
          [Adresse de l&apos;entreprise]<br />
          [Numéro de téléphone]<br />
          [Adresse e-mail]<br />
          [Numéro d&apos;identification de l&apos;entreprise]
        </p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Directeur de la publication :</h2>
        <p>[Nom du directeur de la publication]</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Hébergeur du site :</h2>
        <p>[Nom de l&apos;hébergeur]<br />
          [Adresse de l&apos;hébergeur]<br />
          [Numéro de téléphone de l&apos;hébergeur]
        </p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Propriété intellectuelle :</h2>
        <p>L&apos;ensemble du contenu présent sur le site www.negosud.com est la propriété exclusive de NégoSud, sauf
          indication contraire.</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Protection des données personnelles :</h2>
        <p>Les informations collectées sont traitées dans le respect de la réglementation en vigueur. Consultez
          notre <a
            href="">Politique de confidentialité</a> pour plus
          d&apos;informations.</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Cookies :</h2>
        <p>Notre site utilise des cookies. En continuant à naviguer, vous acceptez l&apos;utilisation de ces cookies.
          Consultez notre <a
            href="">Politique relative aux cookies</a> pour plus
          d&apos;informations.</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Contact :</h2>
        <p>Pour toute question, contactez-nous à l&apos;adresse e-mail : <a
          href="mailto:adresse-e-mail-de-contact">adresse-e-mail-de-contact</a> ou par téléphone au <a
          href="tel:+numerodetelephone">numerodetelephone</a>.</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Crédits :</h2>
        <p>Le site a été créé par [nom de l&apos;agence ou du développeur].</p>
      </section>
      <section>
        <h2 className="text-xl font-semibold mb-4">Date de dernière mise à jour :</h2>
        <p>[Date de la dernière mise à jour du site]</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Conditions Générales d&apos;Utilisation :</h2>
        <p>En accédant à ce site web, vous acceptez de vous conformer aux présentes Conditions Générales d&apos;Utilisation
          (CGU).
          Veuillez consulter notre <a href="lien-vers-cgu">page CGU</a> pour plus de détails sur les modalités
          d&apos;utilisation de notre site.</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Contact :</h2>
        <p>Pour toute question, contactez-nous à l&apos;adresse e-mail : <a
          href="mailto:adresse-e-mail-de-contact">adresse-e-mail-de-contact</a> ou par téléphone au <a
          href="tel:+numerodetelephone">numerodetelephone</a>.</p>
      </section>
      <section className="mb-8">
        <h2 className="text-xl font-semibold mb-4">Crédits :</h2>
        <p>Le site a été créé par [nom de l&apos;agence ou du développeur].</p>
      </section>
      <section>
        <h2 className="text-xl font-semibold mb-4">Date de dernière mise à jour :</h2>
        <p>[Date de la dernière mise à jour du site]</p>
      </section>
    </div>
  <Footer />
</>
)
}