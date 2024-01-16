const API_BASE_URL = "https://localhost:7281/api";

export async function getProducts() {
  const response = await fetch(`${API_BASE_URL}/Product`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des produits');
  }

  return await response.json();
}

export async function getProductByID(id) {
  const response = await fetch(`${API_BASE_URL}/Product/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  
  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des produits');
  }

  return await response.json();
}

/*
import { useEffect } from "react";
import { getProductByID } from "../../../../services/api/products/productService"

const ProductPage = () => {
const productId = null;
    useEffect(() => {
        const fetchData = async () => {
            try {
                const productData = await getProductByID(productId);
            }
        }
    })

    return ()
}
*/