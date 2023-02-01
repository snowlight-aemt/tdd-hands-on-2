package accounting.test;

import accounting.Order;
import accounting.OrderView;
import accounting.OrderViewAggregator;
import accounting.ShopReader;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.UUID;


@SuppressWarnings("NewClassNamingConvention")
public class OrderViewAggregator_specs {

    @Test
    void sut_localizes_Pending_status() {
        // arrange
        ShopReader shopReader = id -> Optional.empty();
        var sut = new OrderViewAggregator(shopReader);

        // 세터가 없음.
        Order order = new Order();
        try {
            Field id = Order.class.getDeclaredField("id");
            id.setAccessible(true);
            id.set(order, UUID.randomUUID());

            Field userId = Order.class.getDeclaredField("userId");
            userId.setAccessible(true);
            userId.set(order, UUID.randomUUID());

            Field shopId = Order.class.getDeclaredField("shopId");
            shopId.setAccessible(true);
            shopId.set(order, UUID.randomUUID());

            Field itemId = Order.class.getDeclaredField("itemId");
            itemId.setAccessible(true);
            itemId.set(order, UUID.randomUUID());

            Field price = Order.class.getDeclaredField("price");
            price.setAccessible(true);
            price.set(order, new BigDecimal(100000));

            Field status = Order.class.getDeclaredField("status");
            status.setAccessible(true);
            status.set(order, "Pending");

            Field placedAtUtc = Order.class.getDeclaredField("placedAtUtc");
            placedAtUtc.setAccessible(true);
            placedAtUtc.set(order, LocalDateTime.now());

        } catch (NoSuchFieldException | IllegalAccessException e) {
            throw new RuntimeException(e);
        }

        // act
        Iterable<OrderView> orderViews = sut.aggregateViews(List.of(order));

        // assert
        List<OrderView> list = new ArrayList<>();
        orderViews.forEach(list::add);
        Assertions.assertEquals("보류", list.get(0).status());
    }

    @Test
    void sut_localizes_AwaitPayment_status() {
        // arrange
        ShopReader shopReader = id -> Optional.empty();
        var sut = new OrderViewAggregator(shopReader);

        // 세터가 없음.
        Order order = new Order();
        try {
            Field id = Order.class.getDeclaredField("id");
            id.setAccessible(true);
            id.set(order, UUID.randomUUID());

            Field userId = Order.class.getDeclaredField("userId");
            userId.setAccessible(true);
            userId.set(order, UUID.randomUUID());

            Field shopId = Order.class.getDeclaredField("shopId");
            shopId.setAccessible(true);
            shopId.set(order, UUID.randomUUID());

            Field itemId = Order.class.getDeclaredField("itemId");
            itemId.setAccessible(true);
            itemId.set(order, UUID.randomUUID());

            Field price = Order.class.getDeclaredField("price");
            price.setAccessible(true);
            price.set(order, new BigDecimal(100000));

            Field status = Order.class.getDeclaredField("status");
            status.setAccessible(true);
            status.set(order, "AwaitingPayment");

            Field placedAtUtc = Order.class.getDeclaredField("placedAtUtc");
            placedAtUtc.setAccessible(true);
            placedAtUtc.set(order, LocalDateTime.now());

        } catch (NoSuchFieldException | IllegalAccessException e) {
            throw new RuntimeException(e);
        }

        // act
        Iterable<OrderView> orderViews = sut.aggregateViews(List.of(order));

        // assert
        List<OrderView> list = new ArrayList<>();
        orderViews.forEach(list::add);
        Assertions.assertEquals("결제대기", list.get(0).status());
    }
}
