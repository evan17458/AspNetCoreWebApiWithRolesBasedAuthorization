專案概述
本專案以 ASP.NET Core 8.0 為基底，實作了一套旅遊路線管理與購物流程的 RESTful API。主要功能包含：

旅遊路線（TouristRoutes）管理：提供旅遊路線的查詢、建立、更新、刪除以及圖片管理等功能。

購物車（ShoppingCart）：可將旅遊路線加入購物車、移除購物車商品，以及結帳等操作。

訂單（Orders）管理：提供建立與查詢訂單的功能。

使用者系統（Account）：

使用者可註冊、登入並取得 JSON Web Token（JWT）

角色管理：支援新增角色、指派角色給使用者，並以角色進行權限控管

這些 API 皆有對應的路由，例如：

/api/TouristRoutes

/api/ShoppingCart

/api/Orders

/api/Account（登入、註冊、角色指派等）

並且搭配 Swagger（OpenAPI） 做自動化 API 文件的產生，方便測試與維護。

核心技術與套件
.NET 8（C#）

建置 RESTful API，支援各項 CRUD 需求及商業邏輯

使用預設的 Controller 與路由屬性實現分層

Entity Framework Core 8

採用 Code-First 或 Database-First 模式（視需求而定）

搭配 Npgsql 操作 PostgreSQL 資料庫

Microsoft.AspNetCore.Authentication.JwtBearer

透過 JWT 實現 Token 驗證機制，提供註冊/登入功能，並藉由 Claims 與角色（Roles）做權限管控

AutoMapper

主要用於在 DTO（Data Transfer Objects）與資料模型之間做對應，簡化屬性映射

Serilog + Serilog.Sinks.Console / Serilog.Sinks.File

實現日誌（Logging）功能，將執行紀錄輸出至主控台或檔案

Swagger (Swashbuckle.AspNetCore)

自動產生 API 文件，可透過 Swagger UI 做接口測試

主要功能說明
TouristRoutes (旅遊路線)

GET /api/TouristRoutes：可依關鍵字、分頁條件、排序等查詢旅遊路線

POST /api/TouristRoutes：建立新的旅遊路線

GET /api/TouristRoutes/{touristRouteId}：查詢單一旅遊路線

PUT /api/TouristRoutes/{touristRouteId} / PATCH /api/TouristRoutes/{touristRouteId}：更新旅遊路線

DELETE /api/TouristRoutes/{touristRouteId}：刪除旅遊路線

另外亦支援對 TouristRoutePictures 進行 CRUD 操作

ShoppingCart (購物車)

GET /api/ShoppingCart：取得使用者的購物車資訊

POST /api/ShoppingCart/items：將旅遊路線商品加入購物車

DELETE /api/ShoppingCart/items/{itemId}：刪除購物車中的某個商品

DELETE /api/ShoppingCart/items/({itemIDs})：支援一次移除多個商品

POST /api/ShoppingCart/checkout：結帳並產生訂單

Orders (訂單)

GET /api/Orders：列出目前使用者歷史訂單清單

GET /api/Orders/{orderId}：查詢指定訂單詳情

POST /api/Orders/{orderId}/placeOrder：確認並下訂

Account (使用者與角色系統)

POST /api/Account/register：註冊新帳號

POST /api/Account/login：使用者登入取得 JWT

POST /api/Account/add-role：建立新角色

POST /api/Account/assign-role：指派角色給指定使用者

透過 [Authorize(Roles = "...")] 屬性控制不同角色的操作權限

專案架構與流程
Controllers 層：負責 API 路由與動作處理，接收並回應請求。

Services / Business Logic 層：封裝資料存取與邏輯運算；例如，購物車加購物品前先檢查存貨或規則等。

Repositories / Data Access 層：藉由 EF Core 與資料庫互動，並提供基本的 CRUD 介面。

Models / DTOs：包含實際的資料庫模型（Entity）、資料傳輸用的 DTO（Data Transfer Object），以及 ViewModel 等不同層次的資料定義。

Infrastructure：包含各種跨領域的工具或設定，如日志（Logging）、DI 容器、AutoMapper 映射設定等。

