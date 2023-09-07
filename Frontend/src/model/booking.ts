import {Hairdresser} from '@/model/hairdresser';
import {Service} from '@/model/service';
import {SpecialOffer} from '@/model/specialOffer';

export interface Booking {
    id: number,
    hairdresserId: number,
    serviceId: number,
    specialOfferId: number,
    date: string,
    time: string,
}

export interface State {
    bookings: Booking[];
}
