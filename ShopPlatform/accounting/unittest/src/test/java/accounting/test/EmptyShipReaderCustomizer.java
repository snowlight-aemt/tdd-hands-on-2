package accounting.test;

import accounting.ShopReader;
import autoparams.customization.Customizer;
import autoparams.generator.ObjectContainer;
import autoparams.generator.ObjectGenerator;

import java.util.Optional;

public class EmptyShipReaderCustomizer implements Customizer {

    @Override
    public ObjectGenerator customize(ObjectGenerator generator) {
        return ((query, context) -> query.getType().equals(ShopReader.class)
                ? new ObjectContainer(getShopReader())
                : generator.generate(query, context));
    }

    private ShopReader getShopReader() {
        return id -> Optional.empty();
    }
}
