package accounting.test;

import accounting.Order;
import accounting.OrderReader;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.UUID;

public class InMemoryOrderRepository extends ArrayList<Order> implements OrderReader {
    @Override
    public Iterable<Order> getOrdersPlacedIn(UUID shopId,
                                             LocalDateTime placedAtUtcStartInclusive,
                                             LocalDateTime placedAtUtcEndExclusive) {
        return stream()
                .filter(x -> x.getShopId().equals(shopId))
                .filter(x -> !x.getPlacedAtUtc().isBefore(placedAtUtcStartInclusive))
                .filter(x -> x.getPlacedAtUtc().isBefore(placedAtUtcEndExclusive))
                .toList();

    }
}
