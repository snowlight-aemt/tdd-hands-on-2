package accounting.test;

import accounting.Order;
import accounting.OrderView;
import accounting.OrderViewAggregator;
import accounting.ShopReader;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.UUID;


@SuppressWarnings("NewClassNamingConvention")
public class OrderViewAggregator_specs {

    @ParameterizedTest
    @CsvSource({"Pending, 보류", "AwaitingPayment, 결제대기", "AwaitingShipment, 배송대기", "Completed, 완료"})
    void sut_localizes_status(String status, String localizedStatus) {
        // arrange
        ShopReader shopReader = id -> Optional.empty();
        var sut = new OrderViewAggregator(shopReader);

        // 세터가 없음.
        Order order = getOrder(status);

        // act
        Iterable<OrderView> orderViews = sut.aggregateViews(List.of(order));

        // assert
        List<OrderView> list = new ArrayList<>();
        orderViews.forEach(list::add);
        Assertions.assertEquals(localizedStatus, list.get(0).status());
    }

    private static Order getOrder(String status) {
        Order order = new Order();
        setField(order, "id", UUID.randomUUID());
        setField(order, "userId", UUID.randomUUID());
        setField(order, "shopId", UUID.randomUUID());
        setField(order, "itemId", UUID.randomUUID());
        setField(order, "price", new BigDecimal(100000));
        setField(order, "status", status);
        setField(order, "placedAtUtc", LocalDateTime.now());
        return order;
    }

    private static void setField(Order order, String name, Object value) {
        try {
            Field id = Order.class.getDeclaredField(name);
            id.setAccessible(true);
            id.set(order, value);
        } catch (NoSuchFieldException | IllegalAccessException e) {
            throw new RuntimeException(e);
        }
    }
}
