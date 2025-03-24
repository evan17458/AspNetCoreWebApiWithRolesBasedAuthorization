using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiWithRoleAuthentication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47119"),
                column: "Rating",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("a1fd0bee-0afc-4586-96c8-f46b7c99d2a0"),
                column: "Rating",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("fb6d4f10-79ed-4aff-a915-4ce29dc9c7e1"),
                column: "Rating",
                value: 3.0);

            migrationBuilder.InsertData(
                table: "TouristRoutes",
                columns: new[] { "Id", "CreateTime", "DepartureCity", "DepartureTime", "Description", "DiscountPresent", "Features", "Fees", "Notes", "OriginalPrice", "Rating", "Title", "TravelDays", "TripType", "UpdateTime" },
                values: new object[,]
                {
                    { new Guid("a4b5c6d7-e8f9-0123-4567-89a0b1c2d3e4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【經典歐洲】巴黎鐵塔|羅浮宮|少女峰|盧塞恩湖|米蘭大教堂|威尼斯水都|佛羅倫薩|羅馬競技場|梵蒂岡|精選五星酒店|米其林餐廳", 0.080000000000000002, "", "", "", 19999.99m, 5.0, "歐洲法國+瑞士+意大利12日10晚跟團遊(5鑽)", null, null, null },
                    { new Guid("b0c1d2e3-f4a5-6789-0123-456789abcdef"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【楓葉國度】尼亞加拉大瀑布|千島湖|CNT塔|魁北克古城|楓糖農場|特色冰酒莊園|國家博物館|精選酒店|舒適巴士|專業領隊", 0.080000000000000002, "", "", "", 8899.99m, 4.0, "加拿大東海岸多倫多+渥太華+蒙特利爾7日6晚跟團遊(4鑽)", null, null, null },
                    { new Guid("b2c3d4e5-6f67-4891-80a1-c2d3e4f5a6b7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【熱帶海島度假】酒店自選|大皇宮+玉佛寺+普吉島沙灘|機票+酒店套餐|輕鬆自由行", null, "", "", "", 4500.75m, 3.0, "泰國曼谷+普吉島6日5晚自由行", null, null, null },
                    { new Guid("b2c3d4e5-f6f7-4891-a0b1-c2d3e4f5a6b7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【品質保證】大皇宮+玉佛寺|水上市場|芭堤雅海灘|金沙島浮潛|網紅夜市|泰式按摩體驗|特色美食|專業領隊貼心服務", 0.20000000000000001, "", "", "", 3899.99m, 4.0, "泰國曼谷+芭堤雅5日4晚跟團遊(4鑽)", null, null, null },
                    { new Guid("c0d1e2f3-a4b5-6789-0123-456789abcdef"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【精選酒店】悉尼歌劇院|藍山國家公園|大堡礁一日遊|十二門徒|企鵝歸巢|華納電影世界|可倫賓野生動物園|特色農場體驗", 0.10000000000000001, "", "", "", 12599.99m, 4.0, "澳洲悉尼+墨爾本+黃金海岸8日7晚自由行·『尊享之旅』", null, null, null },
                    { new Guid("c4d5e6f7-a8b9-0123-4567-890123456789"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【奇幻之旅】藍色清真寺|聖索菲亞大教堂|博斯普魯斯海峽|棉花堡|以弗所古城|熱氣球之旅|洞穴酒店|特色土耳其浴|美食饗宴", 0.12, "", "", "", 9999.95m, 5.0, "土耳其伊斯坦布爾+卡帕多奇亞10日8晚跟團遊(5鑽)", null, null, null },
                    { new Guid("d4f7a9e1-5c3b-4e8d-b2a6-7f9c1e3d5a2b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【歐洲經典之旅】15人精品團|含簽證+全程餐|鬥獸場+聖馬可廣場+比薩斜塔|贈送貢多拉遊船", 0.20000000000000001, "", "", "", 14500.00m, 5.0, "義大利羅馬+佛羅倫斯+威尼斯10日深度遊(5鑽)", null, null, null },
                    { new Guid("d8e9f0a1-b2c3-4567-8901-23456789abcd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【度假勝地】烏布皇宮|海神廟|庫塔海灘|金巴蘭海灘|聖猴森林|百度庫火山|下午茶體驗|SPA按摩|五星度假村|私人泳池別墅", 0.17999999999999999, "", "", "", 5899.99m, 5.0, "印度尼西亞巴厘島6日5晚自由行·『熱帶天堂』", null, null, null },
                    { new Guid("e2f3a4b5-c6d7-8901-2345-6789abcdef01"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【深度遊】台北101|九份老街|日月潭|阿里山日出|墾丁海灘|花蓮太魯閣|夜市美食|溫泉體驗|專業導遊|舒適住宿", 0.10000000000000001, "", "", "", 5299.95m, 4.0, "台灣環島8日7晚跟團遊(4鑽)", null, null, null },
                    { new Guid("e8c2b5f9-12ab-4d7e-9f33-6a8b9c4d2e1f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【純玩無購物】20人小團|全程美食+溫泉體驗|富士山+金閣寺+大阪城|升級新幹線|專業中文導遊", 0.14999999999999999, "", "", "", 8999.50m, 4.0, "日本東京+大阪+京都7日跟團遊(5鑽)", null, null, null },
                    { new Guid("e8f9d0c1-b2a3-4567-89e0-f1c2d3e4f5a6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【雙城遊】聖淘沙名勝世界|環球影城+SEA海洋館|魚尾獅公園|濱海灣花園|雲頂高原|吉隆坡雙子塔|精選五星酒店|機場接送", 0.12, "", "", "", 6799.95m, 5.0, "新加坡+馬來西亞6日5晚自由行『豪華體驗』", null, null, null },
                    { new Guid("f6a7b8c9-d0e1-2345-6789-abcdef012345"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "【超值優惠】明洞購物|南山塔|景福宮|北村韓屋村|濟州島火山口|城山日出峰|泰迪熊博物館|韓服體驗|特色美食|品質住宿", 0.14999999999999999, "", "", "", 4599.99m, 4.0, "韓國首爾+濟州島5日4晚跟團遊(4鑽)", null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("a4b5c6d7-e8f9-0123-4567-89a0b1c2d3e4"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-0123-456789abcdef"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-6f67-4891-80a1-c2d3e4f5a6b7"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6f7-4891-a0b1-c2d3e4f5a6b7"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("c0d1e2f3-a4b5-6789-0123-456789abcdef"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("c4d5e6f7-a8b9-0123-4567-890123456789"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("d4f7a9e1-5c3b-4e8d-b2a6-7f9c1e3d5a2b"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-8901-23456789abcd"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("e2f3a4b5-c6d7-8901-2345-6789abcdef01"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("e8c2b5f9-12ab-4d7e-9f33-6a8b9c4d2e1f"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("e8f9d0c1-b2a3-4567-89e0-f1c2d3e4f5a6"));

            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-6789-abcdef012345"));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47119"),
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("a1fd0bee-0afc-4586-96c8-f46b7c99d2a0"),
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("fb6d4f10-79ed-4aff-a915-4ce29dc9c7e1"),
                column: "Rating",
                value: null);
        }
    }
}
