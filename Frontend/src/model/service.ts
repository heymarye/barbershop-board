export interface Service {
  id: number,
  title: string,
  price: number,
  duration: string
}

export interface State {
  Services: Service[];
}
