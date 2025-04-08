import { StatusResponse } from './../Interface/StatusResponse';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { api_url } from '../app.config';
import { Review } from '../Interface/Review';
import { ReviewDto } from '../Interface/ReviewDto';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private apiUrl = `${api_url}/review`;

  constructor(private http: HttpClient) {}

  getAllReviews(): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/All`);
  }

  getReviewById(id: string): Observable<Review> {
    return this.http.get<Review>(`${this.apiUrl}/GetReviewById/${id}`);
  }

  getReviewsByProductId(productId: string): Observable<Review[]> {
    return this.http.get<Review[]>(
      `${this.apiUrl}/getReviewsByProductId/${productId}`
    );
  }

  createReview(review: ReviewDto): Observable<StatusResponse> {
    return this.http.post<StatusResponse>(
      `${this.apiUrl}/CreateReview`,
      review
    );
  }

  updateReview(id: string, review: Partial<Review>): Observable<any> {
    return this.http.put(`${this.apiUrl}/UpdateReview/${id}`, review);
  }

  deleteReview(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteReview/${id}`);
  }
}
