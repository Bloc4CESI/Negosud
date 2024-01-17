import React from "react";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import "../globals.css";

// Importe tes données de panier ici, par exemple :
// import { products, calculateTotal } from "../path/to/your/cartData";

export default function Cart() {
  // Récupère la liste des produits et le total du panier
  // Remplace les valeurs factices ci-dessous par tes données réelles
  const products = [
    { id: 1, name: "Produit 1", price: 20 },
    { id: 2, name: "Produit 2", price: 30 },
    { id: 3, name: "Produit 3", price: 80 },
    // ... Ajoute d'autres produits selon tes besoins
  ];

  const total = products.reduce((acc, product) => acc + product.price, 0);

  return (
    <>
      <Header />

      <div className="container mx-auto mt-8 flex">
        {/* Section gauche - Liste des produits */}
        <div className="w-2/3 pr-8">
          <h2 className="text-2xl font-bold mb-4">Panier</h2>
          <ul>
            {products.map((product, index) => (
              <li
                key={product.id}
                className={`flex justify-between my-4 ${
                  index < products.length ? "bg-gray-200 rounded-full py-2 px-4" : ""
                }`}
              >
                <span>{product.name}</span>
                <span>{product.price} €</span>
              </li>
            ))}
          </ul>
        </div>

        {/* Section droite - Total et bouton commander */}
        <div className="w-1/3">
          <div className="bg-gray-200 p-4 rounded-lg">
            <h2 className="text-xl font-bold mb-4">Total du Panier</h2>
            <p className="flex justify-between">
              <span>Total :</span>
              <span>{total} €</span>
            </p>
            <button className="mt-4 bg-gray-900 text-white px-4 py-2 rounded-full">
              Commander
            </button>
          </div>
        </div>
      </div>

      <Footer />
    </>
  );
}
