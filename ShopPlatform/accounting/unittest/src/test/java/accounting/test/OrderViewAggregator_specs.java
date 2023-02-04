package accounting.test;

import accounting.Order;
import accounting.OrderView;
import accounting.OrderViewAggregator;
import autoparams.CsvAutoSource;
import autoparams.customization.Customization;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;


@SuppressWarnings("NewClassNamingConvention")
public class OrderViewAggregator_specs {

    @ParameterizedTest
    @CsvAutoSource({"Pending, 보류", "AwaitingPayment, 결제대기", "AwaitingShipment, 배송대기", "Completed, 완료"})
    @Customization(AccountingCustomizer.class)
    void sut_localizes_status(String status, String localizedStatus, OrderViewAggregator sut, Order order) {
        setStatus(order, status);

        Iterable<OrderView> orderViews = sut.aggregateViews(List.of(order));

        List<OrderView> list = new ArrayList<>();
        orderViews.forEach(list::add);
        Assertions.assertEquals(localizedStatus, list.get(0).status());
    }

    private static void setStatus(Order order, Object value) {
        try {
            Field id = Order.class.getDeclaredField("status");
            id.setAccessible(true);
            id.set(order, value);
        } catch (NoSuchFieldException | IllegalAccessException e) {
            throw new RuntimeException(e);
        }
    }
}
