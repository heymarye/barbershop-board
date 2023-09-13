export interface Service {
  id: number,
  title: string,
  description: string,
  price: number,
  duration: string
}

export interface State {
  Services: Service[];
}
