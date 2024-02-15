import React, { useEffect, useState } from "react";
import { getProductByID } from "../../../services/api/products/productService";
import { ProductType } from "../../../services/types/types";
import { useAccount } from "../../../services/api/user/useAccount";
import { postOrderClientLine } from "../../../services/api/products/cart";
import { useRouter } from "next/navigation";

export const FormProduct = ({ id }: { id: number }) => {
  const router = useRouter();
    const [product, setProduct] = useState<ProductType | null>(null);
    const { account} = useAccount();
    const [selectedOption, setSelectedOption] = useState<string>("unitaire"); // État pour stocker la valeur sélectionnée
    const [finalPrice, setFinalPrice] = useState<number | null>(null); // État pour stocker le prix final
    const [quantity, setQuantity] = useState<number>(1); // État pour stocker le prix final

  useEffect(() => {
      const fetchData = async () => {
          try {
              const productData = await getProductByID(id);
              setProduct(productData);
              const initialPrice = productData?.price || null;
              setFinalPrice(initialPrice); // Met à jour le prix final
      } catch (error) {
        console.error('Error during product retrieval:', error);
      }
    };

    fetchData();
  }, [id]);

  console.log(account);
  const handleAddToCart = () => {
    if (account == null) {
      router.push('/login')
    } else {
        postOrderClientLine(product?.id,account?.id,quantity,finalPrice)
    }
  }

  const handleOptionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value; // Récupère la valeur de l'option sélectionnée
    setSelectedOption(value); // Met à jour la valeur sélectionnée

    let newPrice = product?.price;
    let newQuantity = 1;// Prix initial (unitaire)

    if (value !== "unitaire" && product != null) {
        // Prix pour le pack avec une réduction de 10%
        newPrice = product?.price * product?.nbProductBox * 0.9;
        newQuantity = product?.nbProductBox;
    }
    if(newPrice && newQuantity){
        setFinalPrice(newPrice); // Met à jour le prix final
        setQuantity(newQuantity);
    }
}

  const dateFr = product?.dateProduction ? new Date(product.dateProduction).toLocaleDateString('fr-FR', {
    year: "numeric",
    month: "long",
  }) : '';
  console.log(product);

return (
    <section className="overflow-hidden bg-white py-11 font-poppins">
        <div className="max-w-6xl px-4 py-4 mx-auto lg:py-8 md:px-6">
            <div className="flex flex-wrap -mx-4">
                <div className="w-full px-4 md:w-1/2 ">
                    <div className="sticky top-0 z-50 overflow-hidden ">
                        <div className="relative mb-6 lg:mb-10 lg:h-2/4 ">
                            <img src={product?.image} alt=""
                                className="object-cover w-full lg:h-full "/>
                        </div>
                    </div>
                </div>
                <div className="w-full px-4 md:w-1/2 ">
                    <div className="lg:pl-20">
                        <div className="mb-8 ">
                            <span className="flex items-center mb-3">
                            <h2 className=" max-w-xl text-2xl font-bold dark:text-gray-800 md:text-4xl">
                                {product?.name} - </h2>
                                <p className="mt-2 ml-2 text-sm font-bold dark:text-gray-800">{dateFr}</p>
                            </span>
                            <p className="text-base font-bold max-w-md mb-4 dark:text-gray-600">
                            {product?.family.name} signé {product?.home}     
                            </p>
                            <p className="max-w-md mb-8 text-gray-700 dark:text-gray-600">
                            {product?.description}    
                            </p>
                            <p className="inline-block mb-8 text-4xl font-bold dark:text-gray-800 ">
                                <span>{finalPrice}€</span>
                            </p>
                            <p className="text-green-600 text-base font-bold">{product?.stock?.quantity} en stock !</p>
                        </div>
                        <div className="items-center mb-8">
                            <h2 className="w-16 text-xl font-bold dark:text-gray-600">
                                Quantité:</h2>
                            <div className="flex flex-wrap mt-1">
                                <input
                                  type="radio"
                                  id="option1"
                                  name="options"
                                  value="unitaire"
                                  checked={selectedOption === "unitaire"}
                                  onChange={handleOptionChange}
                                  className="hidden"
                                />
                                <label
                                  htmlFor="option1"
                                  className={`py-2 mb-2 mr-1 border w-20 text-center ${
                                    selectedOption === "unitaire"
                                      ? "bg-blue-400 text-white"
                                      : "hover:border-blue-400 dark:border-gray-600 hover:text-blue-600 dark:hover:border-gray-400 dark:text-gray-800"
                                  }`}
                                >
                                  À l'unité
                                </label>
                                <input
                                  type="radio"
                                  id="option2"
                                  name="options"
                                  value={`pack_${product?.nbProductBox}`}
                                  checked={selectedOption !== "unitaire"}
                                  onChange={handleOptionChange}
                                  className="hidden"
                                />
                                <label
                                  htmlFor="option2"
                                  className={`py-2 mb-2 mr-1 border w-20 text-center ${
                                    selectedOption !== "unitaire"
                                      ? "bg-blue-400 text-white"
                                      : "hover:border-blue-400 dark:border-gray-600 hover:text-blue-600 dark:hover:border-gray-400 dark:text-gray-800"
                                  }`}
                                >
                                  Pack de {product?.nbProductBox}
                                </label>
                            </div>
                        </div>
                        <div className="flex flex-wrap items-center -mx-4 ">
                            <div className="w-full px-4 mb-4 lg:w-1/2 lg:mb-0">
                                <button
                                    onClick={handleAddToCart} className="flex items-center justify-center w-full p-4 text-blue-500 border border-blue-500 rounded-md dark:text-gray-200 dark:border-blue-600 hover:bg-blue-600 hover:border-blue-600 hover:text-gray-100 dark:bg-blue-600 dark:hover:bg-blue-700 dark:hover:border-blue-700 dark:hover:text-gray-300">
                                    Ajouter au panier
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
);
};

export default FormProduct;