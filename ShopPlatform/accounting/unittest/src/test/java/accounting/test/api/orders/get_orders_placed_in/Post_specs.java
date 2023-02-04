package accounting.test.api.orders.get_orders_placed_in;

import accounting.Order;
import accounting.OrderReader;
import accounting.OrderView;
import accounting.ShopReader;
import accounting.query.GetOrdersPlacedIn;
import accounting.test.AccountingCustomizer;
import accounting.test.InMemoryOrderRepository;
import autoparams.CsvAutoSource;
import autoparams.customization.Customization;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.context.TestConfiguration;
import org.springframework.boot.test.web.client.TestRestTemplate;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Import;
import org.springframework.context.annotation.Primary;

import java.lang.reflect.Field;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@Import(Post_specs.TestDouble.class)
public class Post_specs {

    @Autowired
    private TestRestTemplate client;

    @Autowired
    private InMemoryOrderRepository orderRepository;


    @TestConfiguration
    public static class TestDouble {
        @Bean
        public InMemoryOrderRepository inMemoryOrderRepository() {
            return new InMemoryOrderRepository();
        }

        @Bean
        @Primary
        public OrderReader orderReaderDouble(InMemoryOrderRepository repository) {
            return repository;
        }

        @Bean
        @Primary
        public ShopReader shopReaderDouble() {
            return id -> Optional.empty();
        }
    }

    @ParameterizedTest
    @CsvAutoSource({
            "2022-08-31T23:59:59, 2022, 9, true",
            "2022-09-01T00:00:00, 2022, 9, true",
    })
    @Customization(AccountingCustomizer.class)
    void sut_correctly_globalizes_time_window(
        String placedAtUtcSource,
        int year,
        int month,
        boolean selected,
        Order order
    ) {
        setPlacedAtUtc(order, LocalDateTime.parse(placedAtUtcSource));
        orderRepository.add(order);
        String path = "/api/orders/get-orders-placed-in";
        var query = new GetOrdersPlacedIn(order.getShopId(), year, month);
        var views = client.postForObject(path, query, OrderView[].class);

        assertEquals(selected, Arrays.stream(views).anyMatch(x -> x.id().equals(order.getId())));
    }

    private void setPlacedAtUtc(Order order, LocalDateTime value) {
        try {
            Field id = Order.class.getDeclaredField("placedAtUtc");
            id.setAccessible(true);
            id.set(order, value);
        } catch (NoSuchFieldException | IllegalAccessException e) {
            throw new RuntimeException(e);
        }
    }

}
