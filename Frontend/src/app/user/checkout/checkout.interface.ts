export interface CheckoutData {
  paymentMethod: number; // 0: COD, 1: Online
  shippingAddress: {
    city: string;
    district: string;
    ward: string;
    street: string;
  };
  recipientName: string;
  phoneNumber: string;
}

export interface City {
  id: string;
  name: string;
}

export interface District {
  id: string;
  name: string;
  cityId: string;
}

export interface Ward {
  id: string;
  name: string;
  districtId: string;
}
