# 旅遊路線購物平台 API 專案

本專案以 **ASP.NET Core 8.0** 為基礎，實作一套 **旅遊路線管理與購物流程** 的 RESTful API。提供旅遊產品的查詢與購買功能，並支援會員登入、購物車、訂單管理及角色權限控制。

# 部署環境資訊

## API 服務
- **主要託管平台**:
  - Azure App Service (Linux 容器)
- **備用平台**:
  - Render 平台

## 資料庫服務
- **PostgreSQL 供應商**:
  - Neon PostgreSQL

## API 文件
- **Swagger 介面**:
  [https://tourapi-ajebcqgpfte5dwex.eastasia-01.azurewebsites.net/swagger/index.html](https://tourapi-ajebcqgpfte5dwex.eastasia-01.azurewebsites.net/swagger/index.html)



## 📌 專案功能概述

- **旅遊路線（TouristRoutes）管理**
  - 查詢、建立、更新、刪除旅遊路線
  - 圖片管理功能

- **購物車（ShoppingCart）**
  - 加入／移除旅遊商品
  - 結帳功能

- **訂單（Orders）管理**
  - 建立訂單
  - 查詢訂單詳情

- **會員系統（Account）**
  - 使用者註冊／登入（JWT 驗證）
  - 角色新增、角色指派
  - 透過角色控制 API 存取權限

---

## 🔗 API 路由範例

| 模組             | 路由範例                                 |
|------------------|------------------------------------------|
| TouristRoutes     | `/api/TouristRoutes`                     |
| ShoppingCart      | `/api/ShoppingCart`                      |
| Orders            | `/api/Orders`                            |
| Account (會員與角色) | `/api/Account/login`、`/api/Account/assign-role` |

✅ 使用 Swagger（OpenAPI）自動產生 API 文件，可直接透過 Swagger UI 測試所有 API。

---

## 🛠️ 使用技術與套件

- **ASP.NET Core 8**
  - 建立 RESTful API，使用 Controller 與屬性路由
  
- **Entity Framework Core 8**
  - 搭配 PostgreSQL + Npgsql 套件
  - 支援 Code-First / Database-First

- **JWT 驗證**
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
  - 使用 Claims 與角色控制存取權限

- **AutoMapper**
  - 用於 Entity 與 DTO 的映射轉換

- **Serilog**
  - 使用 Console 與 File Sink 實現日誌系統

- **Swagger (Swashbuckle.AspNetCore)**
  - 產生互動式 API 文件

---

## 📚 主要 API 功能說明

# API Documentation

## TouristRoutes (旅遊路線)

### Base URL: `/api/TouristRoutes`

| Method | Endpoint                          | Description                  |
|--------|-----------------------------------|------------------------------|
| GET    | `/api/TouristRoutes`              | Get all tourist routes       |
| POST   | `/api/TouristRoutes`              | Create new tourist route     |
| GET    | `/api/TouristRoutes/{touristRouteId}` | Get specific tourist route   |
| PUT    | `/api/TouristRoutes/{touristRouteId}` | Fully update tourist route   |
| PATCH  | `/api/TouristRoutes/{touristRouteId}` | Partially update tourist route |
| DELETE | `/api/TouristRoutes/{touristRouteId}` | Delete tourist route         |

## TouristRoutePictures

### Base URL: `/api/TouristRoutes/{touristRouteId}/pictures`

| Method | Endpoint                                      | Description                          |
|--------|-----------------------------------------------|--------------------------------------|
| GET    | `/api/TouristRoutes/{touristRouteId}/pictures` | Get all pictures for a tourist route |
| POST   | `/api/TouristRoutes/{touristRouteId}/pictures` | Add picture to tourist route        |
| GET    | `/api/TouristRoutes/pictures/{pictureId}`     | Get specific picture                |
| DELETE | `/api/TouristRoutes/pictures/{pictureId}`     | Delete picture                      |

## ShoppingCart (購物車)

### Base URL: `/api/ShoppingCart`

| Method | Endpoint                              | Description                          |
|--------|---------------------------------------|--------------------------------------|
| GET    | `/api/ShoppingCart`                   | Get shopping cart contents           |
| POST   | `/api/ShoppingCart/items`             | Add item to shopping cart            |
| DELETE | `/api/ShoppingCart/items/{itemId}`    | Remove specific item from cart       |
| DELETE | `/api/ShoppingCart/items/({itemIDs})` | Remove multiple items from cart      |
| POST   | `/api/ShoppingCart/checkout`          | Checkout shopping cart               |

## Orders (訂單)

### Base URL: `/api/Orders`

| Method | Endpoint                      | Description                  |
|--------|-------------------------------|------------------------------|
| GET    | `/api/Orders`                 | Get all orders               |
| GET    | `/api/Orders/{orderId}`       | Get specific order           |
| POST   | `/api/Orders/{orderId}/placeOrder` | Place order                 |

## Account (帳戶與角色)

### Base URL: `/api/Account`

| Method | Endpoint                  | Description                  |
|--------|---------------------------|------------------------------|
| POST   | `/api/Account/register`   | Register new account         |
| POST   | `/api/Account/login`      | Login to account            |
| POST   | `/api/Account/add-role`   | Add new role                |
| POST   | `/api/Account/assign-role`| Assign role to user         |