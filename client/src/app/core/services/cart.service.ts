import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, ICartItem } from '../../shared/models/ICart';
import { IProduct } from '../../shared/models/IProduct';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class CartService {
  updateCart(product: IProduct, quantity: number) {
    throw new Error('Method not implemented.');
  }
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);
  itemCount = computed(() => 
    {return this.cart()?.items.reduce((sum, item)=> sum + item.quantity, 0) ?? 0;})

  totals = computed(() => {
      const cart = this.cart();
      if (!cart) return {shipping: 0, subtotal: 0, total: 0}; 
            const subtotal = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
            const shipping = 0;
            const discount = 0;
            const total = subtotal + shipping - discount;
        return {
          shipping: 0,
          discount: 0,
          subtotal: subtotal, 
          total: total
        };
      });
  
  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).pipe(
      map(cart =>{
        this.cart.set(cart);
        return cart;
      })
    );
    
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: cart => this.cart.set(cart)
    });
  }

  addItemToCart(item: ICartItem | IProduct, quantity = 1) {
    const cart = this.cart() ?? this.createCart();
    if (this.isProduct(item)){
      item = this.mapProductToCartItem(item);
    }
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.setCart(cart);
  }

  removeItemFromCart(productId: number, quantity = 1) {
    const cart = this.cart();
    if (!cart) return; 
      const itemIndex = cart.items.findIndex(x => x.productId === productId);
      if (itemIndex !== -1) {
        if (cart.items[itemIndex].quantity > quantity) {
          cart.items[itemIndex].quantity -= quantity;
        } else {
          cart.items.splice(itemIndex, 1);
        }
        if (cart.items.length === 0) {
          this.deleteCart(cart);
        } else {
          this.setCart(cart);
        }
      }
  }
    deleteCart(cart: Cart) {  
      return this.http.delete(this.baseUrl + 'cart?id=' + cart.id).subscribe({
        next: () => {
          localStorage.removeItem('cart_id');
          this.cart.set(null);
        }
      });
    }
    
  
  private addOrUpdateItem(items: ICartItem[], item: ICartItem, quantity: number): ICartItem[] {
    const index = items.findIndex(x => x.productId === item.productId);
    if (index === -1){
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private mapProductToCartItem(items: IProduct): ICartItem{
    return {
      productId: items.id,
      productName: items.name,
      price: items.price,
      quantity: 0,
      pictureUrl: items.pictureUrl,
      brand: items.Brand,
      type: items.Type
    };
  }

  private isProduct(item: ICartItem | IProduct): item is IProduct {
    return (item as IProduct).id !== undefined;
  }

  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }

  // ---RedisHealthCheck.cs---

  checkRedisHealth(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}redisinspection/health`);
  }

  inspectCart(cartId: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}redisinspection/cart/${cartId}`);
  }

}
